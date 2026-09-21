using Evently.Shared.Domain;
using Evently.Shared.Presentation.Endpoints;
using Evently.Shared.Presentation.ApiResults;
using Evently.Modules.Ticketing.Application.Orders.Queries.GetOrder;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Evently.Modules.Ticketing.Application.Orders.ViewModels;

namespace Evently.Modules.Ticketing.Presentation.Orders;

internal sealed class GetOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders/{id}", async (Guid id, ISender sender) =>
        {
            Result<GetOrderViewModel> result = await sender.Send(new GetOrderQuery(id));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Orders);
    }
}
