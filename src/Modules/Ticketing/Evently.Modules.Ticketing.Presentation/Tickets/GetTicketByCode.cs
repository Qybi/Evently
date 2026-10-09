using Evently.Modules.Ticketing.Application.Tickets.Queries.GetTicketByCode;
using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Tickets;

internal sealed class GetTicketByCode : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tickets/code/{code}", async (
            string code,
            IQueryHandler<GetTicketByCodeQuery, TicketViewModel> handler,
            CancellationToken cancellationToken) =>
        {
            Result<TicketViewModel> result = await handler.Handle(new GetTicketByCodeQuery(code), cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetTickets)
        .WithTags(Tags.Tickets);
    }
}
