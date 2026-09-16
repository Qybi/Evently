using Evently.Modules.Ticketing.Application.Carts.AddItemToCart;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Carts;

internal sealed class AddToCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/carts/add", async (AddItemToCartCommand command, ISender sender) =>
        {
            Result result = await sender.Send(command);
            return result.Match(
                () => Results.Ok(),
                ApiResults.Problem
                );

        })
        .WithTags(Tags.Carts);
    }

    internal sealed class Request
    {
        public Guid CustomerId { get; init; }
        public Guid TicketTypeId { get; init; }
        public decimal Quantity { get; init; }
    }
}
