using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events;

public interface IEventRepository
{
    void Insert(Event @event);
}
