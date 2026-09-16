using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;
using Evently.Modules.Ticketing.Domain.Tickets;
using Riok.Mapperly.Abstractions;

namespace Evently.Modules.Ticketing.Application.Tickets.Mappers;

[Mapper]
public static partial class TicketMapper
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial TicketViewModel ToViewModel(this Ticket ticket);
    public static partial IQueryable<TicketViewModel> ProjectToViewModel(this IQueryable<Ticket> tickets);
}
