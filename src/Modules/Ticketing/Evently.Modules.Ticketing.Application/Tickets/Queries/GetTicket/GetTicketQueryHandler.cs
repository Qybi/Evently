using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;
using Evently.Modules.Ticketing.Domain.Tickets.Errors;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Ticketing.Application.Tickets.Queries.GetTicket;

internal sealed class GetTicketQueryHandler(ITicketQueries ticketQueries)
    : IQueryHandler<GetTicketQuery, TicketViewModel>
{
    public async Task<Result<TicketViewModel>> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {
        TicketViewModel? ticket = await ticketQueries.GetTicketAsync(request.TicketId, cancellationToken);

        if (ticket is null)
        {
            return Result.Failure<TicketViewModel>(TicketErrors.NotFound(request.TicketId));
        }

        return ticket;
    }
}
