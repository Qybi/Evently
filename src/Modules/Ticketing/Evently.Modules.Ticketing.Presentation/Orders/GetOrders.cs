using Evently.Modules.Ticketing.Application.Abstractions.Authentication;
using Evently.Modules.Ticketing.Application.Orders.Queries.GetOrders;
using Evently.Modules.Ticketing.Application.Orders.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Orders;

internal sealed class GetOrders : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders", async (
            ICustomerContext customerContext,
            IQueryHandler<GetOrdersQuery, IReadOnlyCollection<GetOrdersViewModel>> handler,
            CancellationToken cancellationToken) =>
        {
            Result<IReadOnlyCollection<GetOrdersViewModel>> result = await handler.Handle(
                new GetOrdersQuery(customerContext.CustomerId),
                cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetOrders)
        .WithTags(Tags.Orders);
    }
}
