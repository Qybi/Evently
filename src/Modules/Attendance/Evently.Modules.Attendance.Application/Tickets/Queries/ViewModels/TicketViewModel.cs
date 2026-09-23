namespace Evently.Modules.Attendance.Application.Tickets.Queries.ViewModels;

public sealed record TicketViewModel(
    Guid Id,
    Guid AttendeeId,
    Guid EventId,
    string Code,
    DateTime? UsedAtUtc);
