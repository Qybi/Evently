using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Categories.Queries.ViewModels;

namespace Evently.Modules.Events.Application.Categories.GetCategory;

public sealed record GetCategoryQuery(Guid CategoryId) : IQuery<CategoryViewModel>;
