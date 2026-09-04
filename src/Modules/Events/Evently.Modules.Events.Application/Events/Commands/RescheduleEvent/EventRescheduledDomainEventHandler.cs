using Evently.Modules.Events.Domain.Events.DomainEvents;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Events.Commands.RescheduleEvent;

internal sealed class EventRescheduledDomainEventHandler : IDomainEventHandler<EventRescheduledDomainEvent>
{
    public Task Handle(EventRescheduledDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
