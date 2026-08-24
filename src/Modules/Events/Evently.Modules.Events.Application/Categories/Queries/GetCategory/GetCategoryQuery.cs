using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Categories.GetCategory;

public sealed record GetCategoryQuery(Guid CategoryId) : IQuery<CategoryViewModel>;
