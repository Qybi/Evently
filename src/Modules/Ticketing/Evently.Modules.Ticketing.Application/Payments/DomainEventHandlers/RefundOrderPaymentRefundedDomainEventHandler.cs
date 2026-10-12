using Evently.Modules.Ticketing.Application.Orders.Commands.RefundOrder;
using Evently.Modules.Ticketing.Domain.Payments.DomainEvents;
using Evently.Shared.Application.Exceptions;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Ticketing.Application.Payments.DomainEventHandlers;

internal sealed class RefundOrderPaymentRefundedDomainEventHandler(ICommandHandler<RefundOrderCommand> handler)
    : DomainEventHandler<PaymentRefundedDomainEvent>
{
    public override async Task Handle(
        PaymentRefundedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await handler.Handle(new RefundOrderCommand(domainEvent.OrderId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(RefundOrderCommand), result.Error);
        }
    }
}
