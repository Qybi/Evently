using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events;

public interface IEventRepository
{
    Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    void Insert(Event @event);
}
