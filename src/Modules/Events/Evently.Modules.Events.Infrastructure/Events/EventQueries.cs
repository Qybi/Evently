using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Application.Events.DTO;
using Evently.Modules.Events.Application.Events.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Events;

internal sealed class EventQueries(EventsDbContext context) : IEventQueries
{
    public Task<EventResponseDto?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Events
            .AsNoTracking()
            .Where(e => e.Id == id)
            .ProjectToDto()
            .SingleOrDefaultAsync(cancellationToken);
    }
}
