using Evently.Modules.Users.Application.Users;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Users.Infrastructure.Repositories;

internal sealed class UserRepository(UsersDbContext context) : IUserRepository
{
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Permissions are part of the User aggregate: grant/revoke need the current set loaded
        return await context.Users
            .Include(u => u.Permissions)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public void Insert(User user)
    {
        foreach (Permission permission in user.Permissions)
        {
            context.Attach(permission);
        }

        context.Users.Add(user);
    }
}
