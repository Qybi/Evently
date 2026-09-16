using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Users.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(Guid UserId, string FirstName, string LastName) : ICommand;
