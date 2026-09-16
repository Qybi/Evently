using Evently.Modules.Ticketing.Domain.Orders;

namespace Evently.Modules.Ticketing.Application.Orders.ViewModels;

public sealed record GetOrderViewModel(
    Guid Id,
    Guid CustomerId,
    OrderStatus Status,
    decimal TotalPrice,
    DateTime CreatedAtUtc,
    IReadOnlyCollection<OrderItemViewModel> OrderItems);
