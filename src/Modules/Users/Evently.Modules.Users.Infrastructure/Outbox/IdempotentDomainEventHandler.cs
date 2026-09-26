using Evently.Modules.Users.Infrastructure.Database;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain.DomainEvents;
using Evently.Shared.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Users.Infrastructure.Outbox;

internal sealed class IdempotentDomainEventHandler<TDomainEvent>(
    IDomainEventHandler<TDomainEvent> decorated,
    UsersDbContext usersDbContext)
    : DomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var outboxMessageConsumer = new OutboxMessageConsumer(domainEvent.Id, decorated.GetType().Name);

        if (await OutboxConsumerExistsAsync(outboxMessageConsumer, cancellationToken))
        {
            return;
        }

        await decorated.Handle(domainEvent, cancellationToken);

        await InsertOutboxConsumerAsync(outboxMessageConsumer, cancellationToken);
    }

    private async Task<bool> OutboxConsumerExistsAsync(
        OutboxMessageConsumer outboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        return await usersDbContext.Set<OutboxMessageConsumer>()
            .AnyAsync(
                o => o.OutboxMessageId == outboxMessageConsumer.OutboxMessageId &&
                     o.Name == outboxMessageConsumer.Name,
                cancellationToken);
    }

    private async Task InsertOutboxConsumerAsync(
        OutboxMessageConsumer outboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        usersDbContext.Set<OutboxMessageConsumer>().Add(outboxMessageConsumer);

        await usersDbContext.SaveChangesAsync(cancellationToken);
    }
}
