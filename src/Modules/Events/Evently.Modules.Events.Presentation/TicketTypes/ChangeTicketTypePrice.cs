using Evently.Modules.Events.Application.TicketTypes.Commands.UpdateTicketTypePrice;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal sealed class ChangeTicketTypePrice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("ticket-types/{id}/price", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateTicketTypePriceCommand> handler,
            CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(new UpdateTicketTypePriceCommand(id, request.Price), cancellationToken);

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.ModifyTicketTypes)
        .WithTags(Tags.TicketTypes);
    }

    internal sealed class Request
    {
        public decimal Price { get; init; }
    }
}
