using Evently.Shared.Domain.Errors;

namespace Evently.Modules.Attendance.Domain.Tickets.Errors;

public static class TicketErrors
{
    public static Error NotFound(Guid ticketId) =>
        Error.NotFound("Tickets.NotFound", $"The ticket with the identifier {ticketId} was not found");

    public static readonly Error InvalidCheckIn = Error.Problem(
        "Tickets.InvalidCheckIn",
        "The ticket check in was invalid");

    public static readonly Error DuplicateCheckIn = Error.Problem(
        "Tickets.DuplicateCheckIn",
        "The ticket was already checked in");
}
