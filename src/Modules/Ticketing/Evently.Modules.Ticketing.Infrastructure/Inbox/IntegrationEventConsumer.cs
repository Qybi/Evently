using Evently.Modules.Ticketing.Infrastructure.Database;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Infrastructure.Inbox;
using Evently.Shared.Infrastructure.Serialization;
using Newtonsoft.Json;

namespace Evently.Modules.Ticketing.Infrastructure.Inbox;

// Wolverine needs a public consumer and does not handle generics, so we have to create an abstract class first, then all concrete consumers will inherit from it.
public abstract class IntegrationEventConsumer<TIntegrationEvent>(TicketingDbContext ticketingDbContext) where TIntegrationEvent : IntegrationEvent
{
    public async Task Handle(TIntegrationEvent integrationEvent)
    {
        var inboxMessage = new InboxMessage
        {
            Id = integrationEvent.Id,
            Type = integrationEvent.GetType().Name,
            Content = JsonConvert.SerializeObject(integrationEvent, SerializerSettings.Instance),
            OccurredOnUtc = integrationEvent.OccurredOnUtc
        };

        ticketingDbContext.Set<InboxMessage>().Add(inboxMessage);

        await ticketingDbContext.SaveChangesAsync();
    }
}
