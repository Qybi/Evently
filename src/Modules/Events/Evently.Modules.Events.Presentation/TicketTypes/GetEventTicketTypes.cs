using Evently.Modules.Events.Application.TicketTypes.Queries.GetEventTicketTypes;
using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal sealed class GetEventTicketTypes : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ticket-types", async (
            Guid eventId,
            IQueryHandler<GetEventTicketTypesQuery, IReadOnlyCollection<TicketTypeViewModel>> handler,
            CancellationToken cancellationToken) =>
        {
            Result<IReadOnlyCollection<TicketTypeViewModel>> result = await handler.Handle(
                new GetEventTicketTypesQuery(eventId),
                cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetTicketTypes)
        .WithTags(Tags.TicketTypes);
    }
}
