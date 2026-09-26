using Evently.Modules.Ticketing.Application.Abstractions.Authentication;
using Evently.Modules.Ticketing.Application.Carts.RemoveItemFromCart;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Carts;

internal sealed class RemoveFromCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("carts/remove", async (Request request, ICustomerContext customerContext, ISender sender) =>
        {
            Result result = await sender.Send(
                new RemoveItemFromCartCommand(customerContext.CustomerId, request.TicketTypeId));

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.RemoveFromCart)
        .WithTags(Tags.Carts);
    }

    internal sealed class Request
    {
        public Guid TicketTypeId { get; init; }
    }
}
