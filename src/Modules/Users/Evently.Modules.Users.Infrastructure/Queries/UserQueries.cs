using Evently.Modules.Users.Application.Users;
using Evently.Modules.Users.Application.Users.Mappers;
using Evently.Modules.Users.Application.Users.Queries.GetUserPermissions;
using Evently.Modules.Users.Application.Users.Queries.ViewModels;
using Evently.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Users.Infrastructure.Queries;

internal sealed class UserQueries(UsersDbContext context) : IUserQueries
{
    public async Task<UserViewModel?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Where(e => e.Id == id)
            .ProjectToViewModel()
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<UserPermissionsViewModel?> GetPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.IdentityId == identityId)
            .Select(u => new UserPermissionsViewModel
            {
                UserId = u.Id,
                Permissions = u.Permissions.Select(p => p.Code).ToList()
            })
            .SingleOrDefaultAsync(cancellationToken);
    }
}
