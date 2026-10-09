using Evently.Modules.Users.IntegrationEvents;
using Evently.Shared.Application.Authorization;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Domain;

namespace Evently.Modules.Users.Presentation.Users.IntegrationEventHandlers;

public sealed class GetUserPermissionsRequestConsumer(IPermissionService permissionService)
    : IRpcConsumer<GetUserPermissionsRequest, GetUserPermissionsReply>
{
    public async Task<GetUserPermissionsReply> Handle(GetUserPermissionsRequest request, CancellationToken cancellationToken)
    {
        Result<PermissionsResponse> result = await permissionService.GetUserPermissionsAsync(request.IdentityId, cancellationToken);

        // The returned reply is sent back to the caller of CallAsync. Basically the server side of an API call that gets sent to the bus and handled by Wolverine
        return result.IsSuccess
            ? new GetUserPermissionsReply(result.Value, null)
            : new GetUserPermissionsReply(null, result.Error);
    }
}
