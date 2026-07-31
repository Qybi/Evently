using Evently.Modules.Events.Application.Events.Queries.ViewModels;
using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;

namespace Evently.Modules.Events.Application.TicketTypes;

public interface ITicketTypeQueries
{
    Task<TicketTypeViewModel?> GetAsync(Guid ticketTypeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TicketTypeViewModel>> GetEventTicketTypesAsync(Guid eventId, CancellationToken cancellationToken = default);
}
