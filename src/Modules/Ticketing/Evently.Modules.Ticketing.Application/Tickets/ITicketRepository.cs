using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Tickets;

namespace Evently.Modules.Ticketing.Application.Tickets;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> GetForEventAsync(Event @event, CancellationToken cancellationToken = default);

    void InsertRange(IEnumerable<Ticket> tickets);
}
