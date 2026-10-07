using Evently.Modules.Events.Application.Categories.Commands.CreateCategory;
using Evently.Shared.Application.Caching;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class CreateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async (
            Request request,
            ICommandHandler<CreateCategoryCommand, Guid> handler,
            ICacheService cacheService,
            CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(new CreateCategoryCommand(request.Name), cancellationToken);

            if (result.IsSuccess)
            {
                await cacheService.RemoveAsync(GetCategories.CacheKey, cancellationToken);
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
