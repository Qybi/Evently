using Evently.Shared.Domain.Errors;

namespace Evently.Modules.Attendance.Domain.Events.Errors;

public static class EventErrors
{
    public static Error NotFound(Guid eventId) =>
        Error.NotFound("Events.NotFound", $"The event with the identifier {eventId} was not found");
}
