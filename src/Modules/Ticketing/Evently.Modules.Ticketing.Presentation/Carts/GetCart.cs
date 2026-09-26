using Evently.Modules.Ticketing.Application.Abstractions.Authentication;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Application.Carts.GetCart;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Carts;

internal sealed class GetCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("carts", async (ICustomerContext customerContext, ISender sender) =>
        {
            Result<Cart> result = await sender.Send(new GetCartQuery(customerContext.CustomerId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetCart)
        .WithTags(Tags.Carts);
    }
}
