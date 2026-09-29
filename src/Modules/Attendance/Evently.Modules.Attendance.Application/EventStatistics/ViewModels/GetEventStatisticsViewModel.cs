namespace Evently.Modules.Attendance.Application.EventStatistics.ViewModels;

public sealed record GetEventStatisticsViewModel(
    Guid EventId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    int TicketsSold,
    int AttendeesCheckedIn,
    IReadOnlyCollection<string> DuplicateCheckInTickets,
    IReadOnlyCollection<string> InvalidCheckInTickets);
