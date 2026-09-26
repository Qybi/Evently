using Evently.Modules.Attendance.Application.Attendees;
using Evently.Modules.Attendance.Application.Attendees.Mappers;
using Evently.Modules.Attendance.Application.Attendees.Queries.ViewModels;
using Evently.Modules.Attendance.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Attendance.Infrastructure.Queries;

internal sealed class AttendeeQueries(AttendanceDbContext context) : IAttendeeQueries
{
    public async Task<AttendeeViewModel?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Attendees
            .AsNoTracking()
            .Where(a => a.Id == id)
            .ProjectToViewModel()
            .SingleOrDefaultAsync(cancellationToken);
    }
}
