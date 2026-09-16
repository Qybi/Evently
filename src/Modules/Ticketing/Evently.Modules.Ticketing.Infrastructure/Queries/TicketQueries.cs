using Evently.Modules.Ticketing.Application.Tickets.Mappers;
using Evently.Modules.Ticketing.Application.Tickets;
using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Queries;

internal sealed class TicketQueries(TicketingDbContext context) : ITicketQueries
{
    public async Task<TicketViewModel?> GetTicketAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Tickets
            .AsNoTracking() 
            .Where(t => t.Id == id)
            .ProjectToViewModel()
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<TicketViewModel?> GetTicketByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await context.Tickets
            .AsNoTracking()
            .Where(t => t.Code == code)
            .ProjectToViewModel()
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TicketViewModel>> GetTicketsForOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await context.Tickets
            .AsNoTracking()
            .Where(t => t.OrderId == orderId)
            .ProjectToViewModel()
            .ToListAsync(cancellationToken);
    }
}
