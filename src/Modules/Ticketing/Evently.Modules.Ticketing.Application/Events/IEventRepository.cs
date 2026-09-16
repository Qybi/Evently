using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Application.Events;

public interface IEventRepository
{
    Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Event @event);
}
