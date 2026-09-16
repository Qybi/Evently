using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;

namespace Evently.Modules.Ticketing.Application.Tickets;

public interface ITicketQueries
{
    Task<TicketViewModel?> GetTicketAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TicketViewModel?> GetTicketByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TicketViewModel>> GetTicketsForOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
}
