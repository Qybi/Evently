using Evently.Modules.Users.Domain.Users.Errors;
using Evently.Shared.Application.Authorization;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Users.Application.Users.Queries.GetUserPermissions;

internal sealed partial class GetUserPermissionsQueryHandler(IUserQueries queries) : IQueryHandler<GetUserPermissionsQuery, PermissionsResponse>
{
    public async Task<Result<PermissionsResponse>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken = default)
    {
        UserPermissionsViewModel? user = await queries.GetPermissionsAsync(request.IdentityId, cancellationToken);

        // A null user means not found. An empty permission list is a valid user with nothing granted.
        if (user is null)
        {
            return Result.Failure<PermissionsResponse>(UserErrors.NotFound(request.IdentityId));
        }

        return new PermissionsResponse(user.UserId, user.Permissions.ToHashSet());
    }
}
