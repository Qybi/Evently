using Evently.Modules.Events.Application.Categories.Commands.CreateCategory;
using Evently.Shared.Application.Caching;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class CreateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async (Request request, ISender sender, ICacheService cacheService) =>
        {
            Result<Guid> result = await sender.Send(new CreateCategoryCommand(request.Name));

            if (result.IsSuccess)
            {
                await cacheService.RemoveAsync(GetCategories.CacheKey);
            }

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.ModifyCategories)
        .WithTags(Tags.Categories);
    }

    internal sealed class Request
    {
        public string Name { get; init; }
    }
}
