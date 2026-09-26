using Evently.Modules.Ticketing.Infrastructure.Database;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Infrastructure.Inbox;
using Evently.Shared.Infrastructure.Serialization;
using MassTransit;
using Newtonsoft.Json;

namespace Evently.Modules.Ticketing.Infrastructure.Inbox;

internal sealed class IntegrationEventConsumer<TIntegrationEvent>(TicketingDbContext ticketingDbContext)
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

        ticketingDbContext.Set<InboxMessage>().Add(inboxMessage);

        await ticketingDbContext.SaveChangesAsync(context.CancellationToken);
    }
}
