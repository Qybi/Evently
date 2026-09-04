using Evently.Modules.Users.Application.Users.Queries.ViewModels;

namespace Evently.Modules.Users.Application.Users;

public interface IUserQueries
{
    Task<UserViewModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
