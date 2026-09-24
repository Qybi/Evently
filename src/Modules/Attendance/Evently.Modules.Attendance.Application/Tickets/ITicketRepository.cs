using Evently.Modules.Attendance.Domain.Tickets;

namespace Evently.Modules.Attendance.Application.Tickets;

public interface ITicketRepository
{
    Task<Ticket?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Ticket ticket);
}
