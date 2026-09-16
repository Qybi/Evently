namespace Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;

public sealed record TicketViewModel(
    Guid Id,
    Guid CustomerId,
    Guid OrderId,
    Guid EventId,
    Guid TicketTypeId,
    string Code,
    DateTime CreatedAtUtc);
