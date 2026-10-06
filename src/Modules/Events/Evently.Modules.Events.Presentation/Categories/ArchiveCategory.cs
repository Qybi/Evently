using Evently.Modules.Events.Application.Categories.Commands.ArchiveCategory;
using Evently.Shared.Application.Caching;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class ArchiveCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id}/archive", async (Guid id, ISender sender, ICacheService cacheService) =>
        {
            Result result = await sender.Send(new ArchiveCategoryCommand(id));

            if (result.IsSuccess)
            {
                await cacheService.RemoveAsync(GetCategories.CacheKey);
            }

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.ModifyCategories)
        .WithTags(Tags.Categories);
    }
}
