using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Repositories;

internal sealed class EventRepository(EventsDbContext context) : IEventRepository
{
    // Write side: tracked entity, so command handlers can mutate and SaveChanges picks it up.
    public Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Events.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Event @event)
    {
        context.Events.Add(@event);
    }
}
