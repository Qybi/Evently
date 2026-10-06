using Evently.Modules.Users.IntegrationEvents;
using Evently.Shared.Application.Authorization;
using Evently.Shared.Domain;
using MassTransit;

namespace Evently.Modules.Users.Presentation.Users.IntegrationEventHandlers;

public sealed class GetUserPermissionsRequestConsumer(IPermissionService permissionService) : IConsumer<GetUserPermissionsRequest>
{
    public async Task Consume(ConsumeContext<GetUserPermissionsRequest> context)
    {
        Result<PermissionsResponse> result = await permissionService.GetUserPermissionsAsync(context.Message.IdentityId, context.CancellationToken);

        // RespondAsync is the response to the IRequestClient. Basically the server side of an API call that gets sent to the bus and handled by masstransit
        if (result.IsSuccess)
        {
            await context.RespondAsync(result.Value);
        }
        else
        {
            await context.RespondAsync(result.Error);
        }
    }
}
