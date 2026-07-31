using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Application.Categories;
using Evently.Modules.Events.Application.Categories.Mappers;
using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Queries;

internal sealed class CategoryQueries(EventsDbContext context) : ICategoryQueries
{
    public async Task<IReadOnlyCollection<CategoryViewModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .AsNoTracking()
            .ProjectToViewModel()
            .ToListAsync(cancellationToken);
    }

    public Task<CategoryViewModel?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Categories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .ProjectToViewModel()
            .SingleOrDefaultAsync(cancellationToken);
    }
}
