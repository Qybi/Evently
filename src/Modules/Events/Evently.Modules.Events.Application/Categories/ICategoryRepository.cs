using Evently.Modules.Events.Domain.Categories;

namespace Evently.Modules.Events.Application.Categories;

public interface ICategoryRepository
{
    Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    void Insert(Category category);
}
