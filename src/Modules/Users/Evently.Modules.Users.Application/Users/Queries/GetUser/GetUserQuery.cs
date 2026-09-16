using Evently.Modules.Users.Application.Users.Queries.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Users.Application.Users.Queries.GetUser;

public sealed record GetUserQuery(Guid UserId) : IQuery<UserViewModel>;
