using Evently.Shared.Domain.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.TicketTypes.DomainEvents;

public sealed class TicketTypeSoldOutDomainEvent(Guid ticketTypeId) : DomainEvent
{
    public Guid TicketTypeId { get; init; } = ticketTypeId;
}
