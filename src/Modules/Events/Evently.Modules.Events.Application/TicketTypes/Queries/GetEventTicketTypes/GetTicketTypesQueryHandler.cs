using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;
using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Application.TicketTypes.Queries.GetEventTicketTypes;

internal sealed class GetEventTicketTypesQueryHandler(ITicketTypeQueries ticketTypeQueries)
    : IQueryHandler<GetEventTicketTypesQuery, IReadOnlyCollection<TicketTypeViewModel>>
{
    public async Task<Result<IReadOnlyCollection<TicketTypeViewModel>>> Handle(
        GetEventTicketTypesQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyCollection<TicketTypeViewModel> ticketTypes =
            await ticketTypeQueries.GetEventTicketTypesAsync(request.EventId, cancellationToken);

        return Result.Success(ticketTypes);
    }
}
