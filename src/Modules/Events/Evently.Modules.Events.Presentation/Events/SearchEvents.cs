using Evently.Modules.Events.Application.Events.Queries.SearchEvents;
using Evently.Modules.Events.Application.Events.Queries.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal sealed class SearchEvents : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/search", async (
            IQueryHandler<SearchEventsQuery, SearchEventsViewModel> handler,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 25) =>
        {
            Result<SearchEventsViewModel> result = await handler.Handle(
                new SearchEventsQuery(categoryId, startDate, endDate, page, pageSize),
                cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.SearchEvents)
        .WithTags(Tags.Events);
    }
}
