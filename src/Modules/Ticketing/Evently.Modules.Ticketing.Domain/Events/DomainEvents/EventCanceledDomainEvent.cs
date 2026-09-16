using Evently.Shared.Domain.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Events.DomainEvents;

public sealed class EventCanceledDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; } = eventId;
}
