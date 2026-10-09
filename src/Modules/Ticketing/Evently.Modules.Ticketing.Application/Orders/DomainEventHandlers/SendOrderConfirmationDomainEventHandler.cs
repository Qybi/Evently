using Evently.Modules.Ticketing.Application.Orders.Queries.GetOrder;
using Evently.Modules.Ticketing.Application.Orders.ViewModels;
using Evently.Modules.Ticketing.Domain.Orders.DomainEvents;
using Evently.Shared.Application.Exceptions;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Ticketing.Application.Orders.DomainEventHandlers;

internal sealed class SendOrderConfirmationDomainEventHandler(IQueryHandler<GetOrderQuery, GetOrderViewModel> handler)
    : DomainEventHandler<OrderCreatedDomainEvent>
{
    public override async Task Handle(
        OrderCreatedDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        Result<GetOrderViewModel> result = await handler.Handle(new GetOrderQuery(notification.OrderId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(GetOrderQuery), result.Error);
        }

        // Send order confirmation notification.
    }
}
