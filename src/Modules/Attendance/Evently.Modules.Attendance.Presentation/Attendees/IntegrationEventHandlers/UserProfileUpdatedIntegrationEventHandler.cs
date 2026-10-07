using Evently.Modules.Attendance.Application.Attendees.Commands.UpdateAttendee;
using Evently.Modules.Users.IntegrationEvents;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Application.Exceptions;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Attendance.Presentation.Attendees.IntegrationEventHandlers;

internal sealed class UserProfileUpdatedIntegrationEventHandler(ICommandHandler<UpdateAttendeeCommand> handler)
    : IntegrationEventHandler<UserProfileUpdatedIntegrationEvent>
{
    public override async Task Handle(
        UserProfileUpdatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await handler.Handle(
            new UpdateAttendeeCommand(
                integrationEvent.UserId,
                integrationEvent.FirstName,
                integrationEvent.LastName),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(UpdateAttendeeCommand), result.Error);
        }
    }
}
