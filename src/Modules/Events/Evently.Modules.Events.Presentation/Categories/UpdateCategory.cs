using Evently.Modules.Events.Application.Categories.UpdateCategory;
using Evently.Shared.Application.Caching;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class UpdateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateCategoryCommand> handler,
            ICacheService cacheService,
            CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(new UpdateCategoryCommand(id, request.Name), cancellationToken);

            if (result.IsSuccess)
            {
                // DB already committed: invalidation must run even if the client disconnected
                await cacheService.RemoveAsync(GetCategories.CacheKey, CancellationToken.None);
            }

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.ModifyCategories)
        .WithTags(Tags.Categories);
    }

    internal sealed class Request
    {
        public string Name { get; init; }
    }
}
