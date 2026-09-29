using Evently.Shared.Domain.DomainEvents;

namespace Evently.Modules.Events.Domain.TicketTypes.DomainEvents;

public sealed class TicketTypePriceChangedDomainEvent(Guid eventId, decimal price) : DomainEvent
{
    public Guid TicketTypeId { get; init; } = eventId;
    public decimal Price { get; init; } = price;
}
