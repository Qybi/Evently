using Evently.Modules.Users.Application.Users;
using Evently.Modules.Users.Application.Users.Mappers;
using Evently.Modules.Users.Application.Users.Queries.GetUserPermissions;
using Evently.Modules.Users.Application.Users.Queries.ViewModels;
using Evently.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Users.Infrastructure.Queries;

internal sealed class UserQueries(UsersDbContext context) : IUserQueries
{
    public Task<UserViewModel?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Users
            .AsNoTracking()
            .Where(e => e.Id == id)
            .ProjectToViewModel()
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<List<UserPermissionViewModel>> GetPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
    {
        return (
                from u in context.Users
                join ur in context.Set<Dictionary<string, object>>("RoleUser")
                    on u.Id equals EF.Property<Guid>(ur, "UserId")
                join rp in context.Set<Dictionary<string, object>>("PermissionRole")
                    on EF.Property<string>(ur, "RolesName") equals EF.Property<string>(rp, "RoleName")
                where u.IdentityId == identityId
                select new UserPermissionViewModel
                {
                    UserId = u.Id,
                    Permission = EF.Property<string>(rp, "PermissionCode")
                })
            .AsNoTracking()
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}
