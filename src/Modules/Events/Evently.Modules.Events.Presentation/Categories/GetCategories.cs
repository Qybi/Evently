using Evently.Modules.Events.Application.Categories.Queries.GetCategories;
using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Evently.Shared.Application.Caching;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class GetCategories : IEndpoint
{
    internal const string CacheKey = "categories";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (
            IQueryHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryViewModel>> handler,
            ICacheService cacheService,
            CancellationToken cancellationToken) =>
        {
            IReadOnlyCollection<CategoryViewModel> cachedCategories = await cacheService.GetAsync<IReadOnlyCollection<CategoryViewModel>>(CacheKey, cancellationToken);

            if (cachedCategories is not null)
            {
                return Results.Ok(cachedCategories);
            }

            Result<IReadOnlyCollection<CategoryViewModel>> result = await handler.Handle(new GetCategoriesQuery(), cancellationToken);

            if (result.IsSuccess)
            {
                await cacheService.SetAsync(CacheKey, result.Value, cancellationToken: cancellationToken);
            }

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetCategories)
        .WithTags(Tags.Categories);
    }
}
