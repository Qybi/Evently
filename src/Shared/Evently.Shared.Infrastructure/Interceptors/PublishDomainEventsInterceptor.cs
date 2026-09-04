using Evently.Shared.Domain;
using Evently.Shared.Domain.DomainEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Shared.Infrastructure.Interceptors;

public sealed class PublishDomainEventsInterceptor(IServiceScopeFactory serviceScopeFactory) : SaveChangesInterceptor
{
    // this interceptor executes after the SaveChangesAsync method is called on the DbContext, and it publishes any domain events that were raised during the transaction. (past tense method)
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await PublishDomainEventsAsync(eventData.Context);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Get all domain events in the current transaction and publish them using the MediatR IPublisher interface. After publishing the events, it clears the domain events from the entities to prevent them from being published again in future transactions.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private async Task PublishDomainEventsAsync(DbContext context)
    {
        var domainEvents = context
            .ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();
        
        using IServiceScope scope = serviceScopeFactory.CreateScope();

        IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}
