using Evently.Modules.Users.Application.Users.Queries.GetUser;
using Evently.Modules.Users.Application.Users.Queries.ViewModels;
using Evently.Modules.Users.PublicApi;
using Evently.Shared.Domain;
using MediatR;

namespace Evently.Modules.Users.Infrastructure.PublicApi;

internal sealed class UsersApi(ISender sender) : IUsersApi
{
    public async Task<UserPublicApiResponse> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        Result<UserViewModel> result = await sender.Send(new GetUserQuery(userId), cancellationToken);

        if (result.IsFailure)
        {
            return null;
        }

        return new UserPublicApiResponse(
            result.Value.Id,
            result.Value.Email,
            result.Value.FirstName,
            result.Value.LastName);
    }
}
