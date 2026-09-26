using Evently.Modules.Ticketing.Application.Tickets.Commands.CreateTicketBatch;
using Evently.Modules.Ticketing.Domain.Orders.DomainEvents;
using Evently.Shared.Application.Exceptions;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Orders.DomainEventHandlers;

internal sealed class CreateTicketsDomainEventHandler(ISender sender)
    : DomainEventHandler<OrderCreatedDomainEvent>
{
    public override async Task Handle(
        OrderCreatedDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new CreateTicketBatchCommand(notification.OrderId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CreateTicketBatchCommand), result.Error);
        }
    }
}
