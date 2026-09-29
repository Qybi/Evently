using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.Application.Events.Commands.CancelEvent;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Application.Exceptions;
using Evently.Shared.Domain;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.Events;

internal sealed class EventCancellationStartedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventCancellationStartedIntegrationEvent>
{
    public override async Task Handle(
        EventCancellationStartedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new CancelEventCommand(integrationEvent.EventId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CancelEventCommand), result.Error);
        }
    }
}
