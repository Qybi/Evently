using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.Categories.Queries.GetCategories;

internal sealed class GetCategoriesQueryHandler(ICategoryQueries categoryQueries)
    : IQueryHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryViewModel>>
{
    public async Task<Result<IReadOnlyCollection<CategoryViewModel>>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyCollection<CategoryViewModel> categories = await categoryQueries.GetAllAsync(cancellationToken);

        return Result.Success(categories);
    }
}
