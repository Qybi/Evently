using Evently.Modules.Users.Application.Users;
using Evently.Modules.Users.Application.Users.Mappers;
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
}
