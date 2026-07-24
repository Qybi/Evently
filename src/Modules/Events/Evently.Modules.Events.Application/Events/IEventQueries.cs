using Evently.Modules.Events.Application.Events.DTO;

namespace Evently.Modules.Events.Application.Events;

public interface IEventQueries
{
    Task<EventResponseDto?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
