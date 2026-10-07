using Evently.Modules.Users.Application.Users.Queries.GetUserPermissions;
using Evently.Shared.Application.Authorization;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Users.Infrastructure.Authorization;

internal sealed class PermissionService(IQueryHandler<GetUserPermissionsQuery, PermissionsResponse> handler)
    : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
    {
        return await handler.Handle(new GetUserPermissionsQuery(identityId), cancellationToken);
    }
}
