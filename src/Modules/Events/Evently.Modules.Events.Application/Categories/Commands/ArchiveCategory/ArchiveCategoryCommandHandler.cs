using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Categories.Errors;

namespace Evently.Modules.Events.Application.Categories.Commands.ArchiveCategory;

internal sealed class ArchiveCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ArchiveCategoryCommand>
{
    public async Task<Result> Handle(ArchiveCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure(CategoryErrors.NotFound(request.CategoryId));
        }

        if (category.IsArchived)
        {
            return Result.Failure(CategoryErrors.AlreadyArchived);
        }

        category.Archive();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
