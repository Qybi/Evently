namespace Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;

public sealed record TicketTypeViewModel(
    Guid Id,
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity);
