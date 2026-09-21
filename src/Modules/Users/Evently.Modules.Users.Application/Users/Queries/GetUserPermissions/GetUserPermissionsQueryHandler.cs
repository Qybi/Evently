using Evently.Modules.Users.Domain.Users.Errors;
using Evently.Shared.Application.Authorization;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Users.Application.Users.Queries.GetUserPermissions;

internal sealed partial class GetUserPermissionsQueryHandler(IUserQueries queries) : IQueryHandler<GetUserPermissionsQuery, PermissionsResponse>
{
    public async Task<Result<PermissionsResponse>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        List<UserPermissionViewModel> permissions = await queries.GetPermissionsAsync(request.IdentityId, cancellationToken);

        if (permissions.Count == 0)
        {
            return Result.Failure<PermissionsResponse>(UserErrors.NotFound(request.IdentityId));
        }

        return new PermissionsResponse(permissions[0].UserId, permissions.Select(p => p.Permission).ToHashSet());
    }
}
