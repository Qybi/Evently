using Evently.Modules.Attendance.Application.Tickets.Queries.ViewModels;

namespace Evently.Modules.Attendance.Application.Tickets;

public interface ITicketQueries
{
    Task<IReadOnlyCollection<TicketViewModel>> GetTicketsForAttendeeAsync(Guid attendeeId, CancellationToken cancellationToken = default);
}
