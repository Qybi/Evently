using Evently.Modules.Attendance.Application.Tickets;
using Evently.Modules.Attendance.Application.Tickets.Mappers;
using Evently.Modules.Attendance.Application.Tickets.Queries.ViewModels;
using Evently.Modules.Attendance.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Attendance.Infrastructure.Queries;

internal sealed class TicketQueries(AttendanceDbContext context) : ITicketQueries
{
    public async Task<IReadOnlyCollection<TicketViewModel>> GetTicketsForAttendeeAsync(
        Guid attendeeId,
        CancellationToken cancellationToken = default)
    {
        return await context.Tickets
            .AsNoTracking()
            .Where(t => t.AttendeeId == attendeeId)
            .ProjectToViewModel()
            .ToListAsync(cancellationToken);
    }
}
