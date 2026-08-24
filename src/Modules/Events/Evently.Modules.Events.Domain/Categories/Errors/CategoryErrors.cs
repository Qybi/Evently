using Evently.Shared.Domain.Errors;

namespace Evently.Modules.Events.Domain.Categories.Errors;

public static class CategoryErrors
{
    public static Error NotFound(Guid categoryId) =>
        Error.NotFound("Categories.NotFound", $"The category with the identifier {categoryId} was not found");

    public static readonly Error AlreadyArchived = Error.Problem(
        "Categories.AlreadyArchived",
        "The category was already archived");
}
