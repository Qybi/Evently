using Evently.Shared.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class EventTicketsArchivedIntegrationEvent : IntegrationEvent
{
    public EventTicketsArchivedIntegrationEvent(Guid id, DateTime occurredOnUtc, Guid eventId) : base(id, occurredOnUtc)
    {
        EventId = eventId;
    }

    public Guid EventId { get; init; }
}
