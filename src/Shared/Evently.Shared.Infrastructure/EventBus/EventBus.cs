using Evently.Shared.Application.EventBus;
using Wolverine;

namespace Evently.Shared.Infrastructure.EventBus;

internal sealed class EventBus(IMessageBus bus) : IEventBus
{
    public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : class, IIntegrationEvent
    {
        await bus.PublishAsync(integrationEvent);
    }
}
