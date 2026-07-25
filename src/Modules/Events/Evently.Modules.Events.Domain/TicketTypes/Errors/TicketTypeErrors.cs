using Evently.Modules.Events.Domain.Abstractions.Errors;

namespace Evently.Modules.Events.Domain.TicketTypes.Errors;

public static class TicketTypeErrors
{
    public static Error NotFound(Guid ticketTypeId) =>
        Error.NotFound("TicketTypes.NotFound", $"The ticket type with the identifier {ticketTypeId} was not found");
}
