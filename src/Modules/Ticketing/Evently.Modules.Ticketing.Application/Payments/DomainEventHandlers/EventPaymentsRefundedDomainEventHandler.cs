using Evently.Modules.Ticketing.Domain.Events.DomainEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Payments.DomainEventHandlers;

internal sealed class EventPaymentsRefundedDomainEventHandler(IEventBus eventBus) : DomainEventHandler<EventPaymentsRefundedDomainEvent>
{
    public override async Task Handle(
        EventPaymentsRefundedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new EventPaymentsRefundedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.EventId),
            cancellationToken);
    }
}
