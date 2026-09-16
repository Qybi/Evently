using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;
using Evently.Modules.Ticketing.Domain.Tickets.Errors;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Ticketing.Application.Tickets.Queries.GetTicketByCode;

internal sealed class GetTicketByCodeQueryHandler(ITicketQueries ticketQueries)
    : IQueryHandler<GetTicketByCodeQuery, TicketViewModel>
{
    public async Task<Result<TicketViewModel>> Handle(GetTicketByCodeQuery request, CancellationToken cancellationToken)
    {
        TicketViewModel? ticket = await ticketQueries.GetTicketByCodeAsync(request.Code, cancellationToken);

        if (ticket is null)
        {
            return Result.Failure<TicketViewModel>(TicketErrors.NotFound(request.Code));
        }

        return ticket;
    }
}
