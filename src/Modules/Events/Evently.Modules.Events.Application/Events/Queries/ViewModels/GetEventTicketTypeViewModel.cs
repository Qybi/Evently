namespace Evently.Modules.Events.Application.Events.Queries.ViewModels;

public sealed record GetEventTicketTypeViewModel(
    Guid TicketTypeId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity);
