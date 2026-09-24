using Evently.Modules.Attendance.Application.Attendees.Queries.ViewModels;

namespace Evently.Modules.Attendance.Application.Attendees;

public interface IAttendeeQueries
{
    Task<AttendeeViewModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
