using Evently.Modules.Ticketing.Application.Abstractions.Authentication;
using Evently.Modules.Ticketing.Application.Orders.Queries.GetOrders;
using Evently.Modules.Ticketing.Application.Orders.ViewModels;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Orders;

internal sealed class GetOrders : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders", async (ICustomerContext customerContext, ISender sender) =>
        {
            Result<IReadOnlyCollection<GetOrdersViewModel>> result = await sender.Send(
                new GetOrdersQuery(customerContext.CustomerId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetOrders)
        .WithTags(Tags.Orders);
    }
}
