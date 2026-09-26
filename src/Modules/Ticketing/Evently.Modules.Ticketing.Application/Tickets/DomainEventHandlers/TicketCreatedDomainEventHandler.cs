using Evently.Modules.Ticketing.Application.Tickets.Queries.GetTicket;
using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;
using Evently.Modules.Ticketing.Domain.Tickets.DomainEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Application.Exceptions;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Tickets.DomainEventHandlers;

internal sealed class TicketCreatedDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<TicketCreatedDomainEvent>
{
    public override async Task Handle(
        TicketCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        Result<TicketViewModel> result = await sender.Send(
            new GetTicketQuery(domainEvent.TicketId),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(GetTicketQuery), result.Error);
        }

        await eventBus.PublishAsync(
            new TicketIssuedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                result.Value.Id,
                result.Value.CustomerId,
                result.Value.EventId,
                result.Value.Code),
            cancellationToken);
    }
}
