using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Categories.Queries.ViewModels;

namespace Evently.Modules.Events.Application.Categories.Queries.GetCategories;

public sealed record GetCategoriesQuery : IQuery<IReadOnlyCollection<CategoryViewModel>>;
