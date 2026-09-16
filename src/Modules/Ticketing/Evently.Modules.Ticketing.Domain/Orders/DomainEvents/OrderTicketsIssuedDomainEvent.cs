using Evently.Shared.Domain.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Orders.DomainEvents;

public sealed class OrderTicketsIssuedDomainEvent(Guid orderId) : DomainEvent
{
    public Guid OrderId { get; init; } = orderId;
}
