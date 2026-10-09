using Evently.Modules.Ticketing.Application.Payments.Commands.RefundPaymentsForEvent;
using Evently.Modules.Ticketing.Domain.Events.DomainEvents;
using Evently.Shared.Application.Exceptions;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Ticketing.Application.Events.DomainEventHandlers;

internal sealed class RefundPaymentsEventCanceledDomainEventHandler(ICommandHandler<RefundPaymentsForEventCommand> handler)
    : DomainEventHandler<EventCanceledDomainEvent>
{
    public override async Task Handle(
        EventCanceledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await handler.Handle(new RefundPaymentsForEventCommand(domainEvent.EventId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(RefundPaymentsForEventCommand), result.Error);
        }
    }
}
