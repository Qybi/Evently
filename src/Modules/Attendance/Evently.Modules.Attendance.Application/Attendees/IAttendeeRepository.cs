using Evently.Modules.Attendance.Domain.Attendees;

namespace Evently.Modules.Attendance.Application.Attendees;

public interface IAttendeeRepository
{
    Task<Attendee?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Attendee attendee);
}
