using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Categories.Errors;

namespace Evently.Modules.Events.Application.Categories.GetCategory;

internal sealed class GetCategoryQueryHandler(ICategoryQueries categoryQueries)
    : IQueryHandler<GetCategoryQuery, CategoryViewModel>
{
    public async Task<Result<CategoryViewModel>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        CategoryViewModel? category = await categoryQueries.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure<CategoryViewModel>(CategoryErrors.NotFound(request.CategoryId));
        }

        return category;
    }
}
