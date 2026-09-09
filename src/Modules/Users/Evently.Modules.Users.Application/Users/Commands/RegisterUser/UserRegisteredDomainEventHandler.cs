using Evently.Modules.Users.Application.Users.Queries.GetUser;
using Evently.Modules.Users.Application.Users.Queries.ViewModels;
using Evently.Modules.Users.Domain.Users.DomainEvents;
using Evently.Modules.Users.IntegrationEvents;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Application.Exceptions;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using MediatR;

namespace Evently.Modules.Users.Application.Users.Commands.RegisterUser;

internal sealed class UserRegisteredDomainEventHandler(ISender sender, IEventBus eventBus) : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Result<UserViewModel> result = await sender.Send(new GetUserQuery(notification.UserId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(GetUserQuery), result.Error);
        }

        await eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                notification.Id,
                notification.OccurredOnUtc,
                result.Value.Id,
                result.Value.Email,
                result.Value.FirstName,
                result.Value.LastName),
            cancellationToken);
    }
}
