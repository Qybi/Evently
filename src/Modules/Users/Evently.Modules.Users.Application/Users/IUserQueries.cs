using Evently.Modules.Users.Application.Users.Queries.GetUserPermissions;
using Evently.Modules.Users.Application.Users.Queries.ViewModels;

namespace Evently.Modules.Users.Application.Users;

public interface IUserQueries
{
    Task<UserViewModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<UserPermissionsViewModel?> GetPermissionsAsync(string identityId, CancellationToken cancellationToken = default);
}
