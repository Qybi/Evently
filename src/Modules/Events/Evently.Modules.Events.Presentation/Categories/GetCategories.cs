using Evently.Modules.Events.Application.Categories.Queries.GetCategories;
using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Evently.Shared.Application.Caching;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class GetCategories : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
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

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Categories);
    }
}
