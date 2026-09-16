using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Ticketing.Application.Tickets.Queries.GetTicketForOrder;

internal sealed class GetTicketsForOrderQueryHandler(ITicketQueries ticketQueries) : IQueryHandler<GetTicketsForOrderQuery, IReadOnlyCollection<TicketViewModel>>
{
    public async Task<Result<IReadOnlyCollection<TicketViewModel>>> Handle(GetTicketsForOrderQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<TicketViewModel> tickets = await ticketQueries.GetTicketsForOrderAsync(request.OrderId, cancellationToken);

        return Result.Success(tickets);
    }
}
