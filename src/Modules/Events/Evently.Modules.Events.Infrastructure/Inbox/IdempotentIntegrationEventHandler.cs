using Evently.Modules.Events.Infrastructure.Database;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Inbox;

internal sealed class IdempotentIntegrationEventHandler<TIntegrationEvent>(
    IIntegrationEventHandler<TIntegrationEvent> decorated,
    EventsDbContext eventsDbContext)
    : IntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    public override async Task Handle(
        TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        var inboxMessageConsumer = new InboxMessageConsumer(integrationEvent.Id, decorated.GetType().Name);

        if (await InboxConsumerExistsAsync(inboxMessageConsumer, cancellationToken))
        {
            return;
        }

        await decorated.Handle(integrationEvent, cancellationToken);

        await InsertInboxConsumerAsync(inboxMessageConsumer, cancellationToken);
    }

    private async Task<bool> InboxConsumerExistsAsync(
        InboxMessageConsumer inboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        return await eventsDbContext.Set<InboxMessageConsumer>()
            .AnyAsync(
                i => i.InboxMessageId == inboxMessageConsumer.InboxMessageId &&
                     i.Name == inboxMessageConsumer.Name,
                cancellationToken);
    }

    private async Task InsertInboxConsumerAsync(
        InboxMessageConsumer inboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        eventsDbContext.Set<InboxMessageConsumer>().Add(inboxMessageConsumer);

        await eventsDbContext.SaveChangesAsync(cancellationToken);
    }
}
