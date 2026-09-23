using Evently.Modules.Attendance.Domain.Events;

namespace Evently.Modules.Attendance.Application.Events;

public interface IEventRepository
{
    Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Event @event);
}
