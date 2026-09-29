using Evently.Modules.Attendance.Application.EventStatistics.ViewModels;

namespace Evently.Modules.Attendance.Application.EventStatistics;

public interface IEventStatisticsQueries
{
    Task<GetEventStatisticsViewModel?> GetAsync(Guid eventId, CancellationToken cancellationToken = default);
}
