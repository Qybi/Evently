using Evently.Modules.Ticketing.Domain.Events.DomainEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Tickets.DomainEventHandlers;

internal sealed class EventTicketsArchivedDomainEventHandler(IEventBus eventBus)
     : DomainEventHandler<EventTicketsArchivedDomainEvent>
{
    public override async Task Handle(
        EventTicketsArchivedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new EventTicketsArchivedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.EventId),
            cancellationToken);
    }
}
