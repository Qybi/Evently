using Evently.Modules.Events.Domain.Events.DomainEvents;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Events.DomainEventHandlers;

internal sealed class EventRescheduledDomainEventHandler(IEventBus eventBus) : DomainEventHandler<EventRescheduledDomainEvent>
{
    public async override Task Handle(EventRescheduledDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new EventRescheduledIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.EventId,
                domainEvent.StartsAtUtc,
                domainEvent.EndsAtUtc),
            cancellationToken);
    }
}
