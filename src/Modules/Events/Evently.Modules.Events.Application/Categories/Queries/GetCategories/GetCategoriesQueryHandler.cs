using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Evently.Modules.Events.Domain.Abstractions;

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
