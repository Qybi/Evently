using Evently.Shared.Domain.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Orders.DomainEvents;

public sealed class OrderCreatedDomainEvent(Guid orderId) : DomainEvent
{
    public Guid OrderId { get; init; } = orderId;
}
