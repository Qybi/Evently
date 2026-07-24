using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Infrastructure.Events;

internal sealed class EventRepository(EventsDbContext context) : IEventRepository
{
    public void Insert(Event @event)
    {
        context.Events.Add(@event);
    }
}
