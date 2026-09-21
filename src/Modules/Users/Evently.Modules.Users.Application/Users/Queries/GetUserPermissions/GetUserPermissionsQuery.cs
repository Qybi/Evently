using Evently.Shared.Application.Authorization;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Users.Application.Users.Queries.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;
