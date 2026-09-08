using Evently.Modules.Events.Application.TicketTypes.Queries.GetTicketType;
using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;
using Evently.Modules.Events.PublicApi;
using Evently.Shared.Domain;
using MediatR;

namespace Evently.Modules.Events.Infrastructure.PublicApi;

internal sealed class EventsApi(ISender sender) : IEventsApi
{
    public async Task<TicketTypePublicApiResponse?> GetTicketTypeAsync(Guid ticketTypeId, CancellationToken cancellationToken)
    {
        Result<TicketTypeViewModel> result = await sender.Send(new GetTicketTypeQuery(ticketTypeId), cancellationToken);
        if (result.IsFailure)
        {
            return null;
        }

        return new TicketTypePublicApiResponse(
            Id: result.Value.Id,
            EventId: result.Value.EventId,
            Name: result.Value.Name,
            Price: result.Value.Price,
            Currency: result.Value.Currency,
            Quantity: result.Value.Quantity
        );
    }
}
