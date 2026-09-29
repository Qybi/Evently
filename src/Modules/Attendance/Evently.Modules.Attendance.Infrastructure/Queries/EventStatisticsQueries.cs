using Evently.Modules.Attendance.Application.EventStatistics;
using Evently.Modules.Attendance.Application.EventStatistics.Mappers;
using Evently.Modules.Attendance.Application.EventStatistics.ViewModels;
using Evently.Modules.Attendance.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Attendance.Infrastructure.Queries;

internal sealed class EventStatisticsQueries(AttendanceDbContext context) : IEventStatisticsQueries
{
    public async Task<GetEventStatisticsViewModel?> GetAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await context.EventStatistics
            .AsNoTracking()
            .Where(es => es.EventId == eventId)
            .ProjectToViewModel()
            .SingleOrDefaultAsync(cancellationToken);
    }
}
