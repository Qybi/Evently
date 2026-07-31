using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.TicketTypes.Errors;

namespace Evently.Modules.Events.Application.TicketTypes.Queries.GetTicketType;

internal sealed class GetTicketTypeQueryHandler(ITicketTypeQueries ticketTypeQueries)
    : IQueryHandler<GetTicketTypeQuery, TicketTypeViewModel>
{
    public async Task<Result<TicketTypeViewModel>> Handle(
        GetTicketTypeQuery request,
        CancellationToken cancellationToken)
    {
        TicketTypeViewModel? ticketType = await ticketTypeQueries.GetAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure<TicketTypeViewModel>(TicketTypeErrors.NotFound(request.TicketTypeId));
        }

        return ticketType;
    }
}
