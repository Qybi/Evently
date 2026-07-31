using Evently.Modules.Events.Application.Categories.Queries.ViewModels;

namespace Evently.Modules.Events.Application.Categories;

public interface ICategoryQueries
{
    Task<IReadOnlyCollection<CategoryViewModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoryViewModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
