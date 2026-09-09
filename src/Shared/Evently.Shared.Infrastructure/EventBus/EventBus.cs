using Evently.Shared.Application.EventBus;
using MassTransit;

namespace Evently.Shared.Infrastructure.EventBus;

internal sealed class EventBus(IBus bus) : IEventBus
{
    public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : class, IIntegrationEvent
    {
        await bus.Publish(integrationEvent, cancellationToken);
    }
}
