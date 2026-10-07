using Evently.Modules.Ticketing.Application.Tickets.Queries.GetTicketForOrder;
using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Tickets;

internal sealed class GetTicketsForOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tickets/order/{orderId}", async (
            Guid orderId,
            IQueryHandler<GetTicketsForOrderQuery, IReadOnlyCollection<TicketViewModel>> handler,
            CancellationToken cancellationToken) =>
        {
            Result<IReadOnlyCollection<TicketViewModel>> result = await handler.Handle(
                new GetTicketsForOrderQuery(orderId),
                cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetTickets)
        .WithTags(Tags.Tickets);
    }
}
