using EventStatisticsEntity = Evently.Modules.Attendance.Domain.Events.EventStatistics;

namespace Evently.Modules.Attendance.Application.EventStatistics;

public interface IEventStatisticsRepository
{
    Task<EventStatisticsEntity?> GetAsync(Guid eventId, CancellationToken cancellationToken = default);

    void Insert(EventStatisticsEntity eventStatistics);
}
