using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Evently.Modules.Events.Domain.Categories;
using Riok.Mapperly.Abstractions;

namespace Evently.Modules.Events.Application.Categories.Mappers;

[Mapper]
public static partial class CategoryMapper
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial CategoryViewModel ToViewModel(this Category category);
    public static partial IQueryable<CategoryViewModel> ProjectToViewModel(this IQueryable<Category> categories);
}
