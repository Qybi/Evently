using Evently.Modules.Events.Application.Categories.Queries.GetCategories;
using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Evently.Modules.Events.Presentation.ApiResults;
using Evently.Shared.Application.Caching;
using Evently.Shared.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal static class GetCategories
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ISender sender, ICacheService cacheService) =>
        {
            IReadOnlyCollection<CategoryViewModel> cachedCategories = await cacheService.GetAsync<IReadOnlyCollection<CategoryViewModel>>("categories");

            if (cachedCategories is not null)
            {
                return Results.Ok(cachedCategories);
            }

            Result<IReadOnlyCollection<CategoryViewModel>> result = await sender.Send(new GetCategoriesQuery());

            if (result.IsSuccess)
            {
                await cacheService.SetAsync("categories", result.Value);
            }

            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.Categories);
    }
}
