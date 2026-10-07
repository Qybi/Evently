using Evently.Modules.Events.Application.TicketTypes.Queries.GetTicketType;
using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal sealed class GetTicketType : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ticket-types/{id}", async (
            Guid id,
            IQueryHandler<GetTicketTypeQuery, TicketTypeViewModel> handler,
            CancellationToken cancellationToken) =>
        {
            Result<TicketTypeViewModel> result = await handler.Handle(new GetTicketTypeQuery(id), cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetTicketTypes)
        .WithTags(Tags.TicketTypes);
    }
}
