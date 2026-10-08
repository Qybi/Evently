using Evently.Modules.Ticketing.Application.Orders.Queries.GetOrder;
using Evently.Modules.Ticketing.Application.Orders.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Orders;

internal sealed class GetOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders/{id}", async (
            Guid id,
            IQueryHandler<GetOrderQuery, GetOrderViewModel> handler,
            CancellationToken cancellationToken) =>
        {
            Result<GetOrderViewModel> result = await handler.Handle(new GetOrderQuery(id), cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetOrders)
        .WithTags(Tags.Orders);
    }
}
