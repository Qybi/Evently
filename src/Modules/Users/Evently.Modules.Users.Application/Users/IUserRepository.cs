using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users;

public interface IUserRepository
{
    Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(User user);
}
