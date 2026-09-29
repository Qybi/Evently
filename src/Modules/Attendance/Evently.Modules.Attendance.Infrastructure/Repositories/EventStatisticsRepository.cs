using Evently.Modules.Attendance.Application.EventStatistics;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Attendance.Infrastructure.Repositories;

internal sealed class EventStatisticsRepository(AttendanceDbContext context) : IEventStatisticsRepository
{
    public async Task<EventStatistics?> GetAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await context.EventStatistics.SingleOrDefaultAsync(es => es.EventId == eventId, cancellationToken);
    }

    public void Insert(EventStatistics eventStatistics)
    {
        context.EventStatistics.Add(eventStatistics);
    }
}
