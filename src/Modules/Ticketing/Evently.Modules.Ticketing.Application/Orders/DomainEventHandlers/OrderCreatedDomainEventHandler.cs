using Evently.Modules.Ticketing.Application.Orders.Queries.GetOrder;
using Evently.Modules.Ticketing.Application.Orders.ViewModels;
using Evently.Modules.Ticketing.Domain.Orders.DomainEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Application.Exceptions;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Orders.DomainEventHandlers;

internal sealed class OrderCreatedDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<OrderCreatedDomainEvent>
{
    public override async Task Handle(
        OrderCreatedDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        Result<GetOrderViewModel> result = await sender.Send(new GetOrderQuery(notification.OrderId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(GetOrderQuery), result.Error);
        }

        await eventBus.PublishAsync(
            new OrderCreatedIntegrationEvent(
                notification.Id,
                notification.OccurredOnUtc,
                result.Value.Id,
                result.Value.CustomerId,
                result.Value.TotalPrice,
                result.Value.CreatedAtUtc,
                result.Value.OrderItems.Select(oi => new OrderItemModel
                {
                    Id = oi.OrderItemId,
                    OrderId = result.Value.Id,
                    TicketTypeId = oi.TicketTypeId,
                    Price = oi.Price,
                    UnitPrice = oi.UnitPrice,
                    Currency = oi.Currency,
                    Quantity = oi.Quantity
                }).ToList()),
            cancellationToken);
    }
}
