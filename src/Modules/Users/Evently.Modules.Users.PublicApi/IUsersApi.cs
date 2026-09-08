namespace Evently.Modules.Users.PublicApi;

public interface IUsersApi
{
    Task<UserPublicApiResponse> GetAsync(Guid userId, CancellationToken cancellationToken = default);
}
