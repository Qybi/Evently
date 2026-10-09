using Evently.Modules.Events.Application.Categories.GetCategory;
using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class GetCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories/{id}", async (
            Guid id,
            IQueryHandler<GetCategoryQuery, CategoryViewModel> handler,
            CancellationToken cancellationToken) =>
        {
            Result<CategoryViewModel> result = await handler.Handle(new GetCategoryQuery(id), cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetCategories)
        .WithTags(Tags.Categories);
    }
}
