using Evently.Modules.Events.Infrastructure.Database;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Infrastructure.Inbox;
using Evently.Shared.Infrastructure.Serialization;
using MassTransit;
using Newtonsoft.Json;

namespace Evently.Modules.Attendance.Infrastructure.Inbox;

internal sealed class IntegrationEventConsumer<TIntegrationEvent>(EventsDbContext eventsDbContext)
    : IConsumer<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    public async Task Consume(ConsumeContext<TIntegrationEvent> context)
    {
        TIntegrationEvent integrationEvent = context.Message;

        var inboxMessage = new InboxMessage
        {
            Id = integrationEvent.Id,
            Type = integrationEvent.GetType().Name,
            Content = JsonConvert.SerializeObject(integrationEvent, SerializerSettings.Instance),
            OccurredOnUtc = integrationEvent.OccurredOnUtc
        };

        eventsDbContext.Set<InboxMessage>().Add(inboxMessage);

        await eventsDbContext.SaveChangesAsync(context.CancellationToken);
    }
}
