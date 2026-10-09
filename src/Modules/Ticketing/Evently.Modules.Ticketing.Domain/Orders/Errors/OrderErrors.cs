using Evently.Shared.Domain.Errors;

namespace Evently.Modules.Ticketing.Domain.Orders.Errors;

public static class OrderErrors
{
    public static Error NotFound(Guid orderId) =>
        Error.NotFound("Orders.NotFound", $"The order with the identifier {orderId} was not found");


    public static readonly Error TicketsAlreadyIssues = Error.Problem(
        "Order.TicketsAlreadyIssued",
        "The tickets for this order were already issued");

    public static readonly Error AlreadyRefunded = Error.Problem(
        "Order.AlreadyRefunded",
        "The order was already refunded");
}
