using Evently.Modules.Attendance.Application.Tickets.Queries.ViewModels;
using Evently.Modules.Attendance.Domain.Tickets;
using Riok.Mapperly.Abstractions;

namespace Evently.Modules.Attendance.Application.Tickets.Mappers;

[Mapper]
public static partial class TicketMapper
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial TicketViewModel ToViewModel(this Ticket ticket);
    public static partial IQueryable<TicketViewModel> ProjectToViewModel(this IQueryable<Ticket> tickets);
}
