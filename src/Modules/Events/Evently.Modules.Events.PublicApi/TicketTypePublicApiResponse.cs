namespace Evently.Modules.Events.PublicApi;

public sealed record TicketTypePublicApiResponse(Guid Id, Guid EventId, string Name, decimal Price, string Currency, decimal Quantity);
