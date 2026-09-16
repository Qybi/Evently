namespace Evently.Modules.Ticketing.Application.Orders.ViewModels;

public sealed record OrderItemViewModel(
    Guid OrderItemId,
    Guid OrderId,
    Guid TicketTypeId,
    decimal Quantity,
    decimal UnitPrice,
    decimal Price,
    string Currency);
