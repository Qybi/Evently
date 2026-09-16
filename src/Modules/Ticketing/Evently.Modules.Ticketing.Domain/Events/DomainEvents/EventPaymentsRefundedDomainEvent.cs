using Evently.Shared.Domain.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Events.DomainEvents;

public sealed class EventPaymentsRefundedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
}
