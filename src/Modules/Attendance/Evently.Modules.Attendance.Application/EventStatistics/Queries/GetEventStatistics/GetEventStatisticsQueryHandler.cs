using Evently.Modules.Attendance.Application.EventStatistics.ViewModels;
using Evently.Modules.Attendance.Domain.Events.Errors;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Attendance.Application.EventStatistics.Queries.GetEventStatistics;

internal sealed class GetEventStatisticsQueryHandler(IEventStatisticsQueries eventStatisticsQueries)
    : IQueryHandler<GetEventStatisticsQuery, GetEventStatisticsViewModel>
{
    public async Task<Result<GetEventStatisticsViewModel>> Handle(
        GetEventStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        GetEventStatisticsViewModel? eventStatistics = await eventStatisticsQueries.GetAsync(
            request.EventId,
            cancellationToken);

        if (eventStatistics is null)
        {
            return Result.Failure<GetEventStatisticsViewModel>(EventErrors.NotFound(request.EventId));
        }

        return eventStatistics;
    }
}
