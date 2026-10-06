using Evently.Modules.Users.IntegrationEvents;
using Evently.Shared.Application.Authorization;
using Evently.Shared.Application.Caching;
using Evently.Shared.Domain;
using Evently.Shared.Domain.Errors;
using MassTransit;

namespace Evently.Modules.Ticketing.Infrastructure.Authorization;

internal sealed class PermissionService(IRequestClient<GetUserPermissionsRequest> requestClient, ICacheService cacheService) : IPermissionService
{
    private static readonly Error NotFound = Error.NotFound(nameof(PermissionService), "User was not found.");
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
    {
        PermissionsResponse? cachedPermissions = await cacheService.GetAsync<PermissionsResponse>(CreateCacheKey(identityId), cancellationToken);

        if (cachedPermissions is not null)
        {
            return Result.Success(cachedPermissions);
        }

        // MassTransit call to call and wait for the response from the Users module. Basically API logic over the message bus.
        // Cool thing is that it acts like an RPC call where I can return multiple types of responses and handle them accordingly.
        Response<PermissionsResponse, Error> response = await requestClient.GetResponse<PermissionsResponse, Error>(new GetUserPermissionsRequest(identityId), cancellationToken);

        if (response.Is(out Response<Error> errorResponse))
        {
            return Result.Failure<PermissionsResponse>(errorResponse.Message);
        }

        if (response.Is(out Response<PermissionsResponse> successResponse))
        {
            await cacheService.SetAsync(CreateCacheKey(identityId), successResponse.Message, CacheExpiration, cancellationToken);

            return Result.Success(successResponse.Message);
        }

        return Result.Failure<PermissionsResponse>(NotFound);
    }

    private static string CreateCacheKey(string identityId) => $"user_permissions:{identityId}";
}
