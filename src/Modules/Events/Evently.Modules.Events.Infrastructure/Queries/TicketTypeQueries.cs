using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Application.TicketTypes;
using Evently.Modules.Events.Application.TicketTypes.Mappers;
using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Queries;

internal sealed class TicketTypeQueries(EventsDbContext context) : ITicketTypeQueries
{
    // Read side: no tracking, projected in SQL, never materializes the entity.
    public Task<TicketTypeViewModel?> GetAsync(Guid ticketTypeId, CancellationToken cancellationToken = default)
    {
        return context.TicketTypes
            .AsNoTracking()
            .Where(t => t.Id == ticketTypeId)
            .ProjectToViewModel()
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TicketTypeViewModel>> GetEventTicketTypesAsync(
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        return await context.TicketTypes
            .AsNoTracking()
            .Where(t => t.EventId == eventId)
            .ProjectToViewModel()
            .ToListAsync(cancellationToken);
    }
}
