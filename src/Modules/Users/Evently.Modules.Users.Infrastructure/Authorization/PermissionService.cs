using Evently.Modules.Users.Application.Users.Queries.GetUserPermissions;
using Evently.Shared.Application.Authorization;
using Evently.Shared.Domain;
using MediatR;

namespace Evently.Modules.Users.Infrastructure.Authorization;

internal sealed class PermissionService(ISender sender) : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        return await sender.Send(new GetUserPermissionsQuery(identityId));
    }
}
