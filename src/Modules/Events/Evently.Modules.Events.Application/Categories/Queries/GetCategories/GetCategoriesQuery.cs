using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Categories.Queries.GetCategories;

public sealed record GetCategoriesQuery : IQuery<IReadOnlyCollection<CategoryViewModel>>;
