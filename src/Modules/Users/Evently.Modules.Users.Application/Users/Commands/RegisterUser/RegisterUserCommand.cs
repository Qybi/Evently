using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Users.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Password, string FirstName, string LastName) : ICommand<Guid>;
