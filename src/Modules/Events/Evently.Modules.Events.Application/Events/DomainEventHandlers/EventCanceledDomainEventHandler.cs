using Evently.Modules.Events.Domain.Events.DomainEvents;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Events.DomainEventHandlers;

internal sealed class EventCanceledDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<EventCanceledDomainEvent>
{
    public override async Task Handle(
        EventCanceledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new EventCanceledIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.EventId),
            cancellationToken);
    }
}
