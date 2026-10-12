using Evently.Modules.Users.IntegrationEvents;
using Evently.Shared.Application.Authorization;
using Evently.Shared.Application.Caching;
using Evently.Shared.Domain;
using Evently.Shared.Infrastructure.EventBus;
using Wolverine;

namespace Evently.Modules.Ticketing.Infrastructure.Authorization;

internal sealed class PermissionService(IMessageBus bus, ICacheService cacheService) : IPermissionService
{
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
    {
        PermissionsResponse? cachedPermissions = await cacheService.GetAsync<PermissionsResponse>(CreateCacheKey(identityId), cancellationToken);

        if (cachedPermissions is not null)
        {
            return Result.Success(cachedPermissions);
        }

        // Wolverine call to call and wait for the reply from the Users module. Basically API logic over the message bus.
        // It acts like an RPC call: Wolverine allows a single reply type, so the reply wraps either the permissions or the error.
        GetUserPermissionsReply reply = await bus.CallAsync(new GetUserPermissionsRequest(identityId), cancellationToken);

        var result = reply.ToResult();

        if (result.IsSuccess)
        {
            await cacheService.SetAsync(CreateCacheKey(identityId), result.Value, CacheExpiration, cancellationToken);
        }

        return result;
    }

    private static string CreateCacheKey(string identityId) => $"user_permissions:{identityId}";
}
