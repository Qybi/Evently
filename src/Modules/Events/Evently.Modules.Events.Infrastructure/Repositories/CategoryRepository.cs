using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Application.Categories;
using Evently.Modules.Events.Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Repositories;

internal sealed class CategoryRepository(EventsDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Categories.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    public void Insert(Category category)
    {
        context.Categories.Add(category);
    }
}
