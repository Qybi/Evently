using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;
using Evently.Modules.Events.Domain.TicketTypes;
using Riok.Mapperly.Abstractions;

namespace Evently.Modules.Events.Application.TicketTypes.Mappers;

[Mapper]
public static partial class TicketTypeMapper
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial TicketTypeViewModel ToViewModel(this TicketType ticketType);
    public static partial IQueryable<TicketTypeViewModel> ProjectToViewModel(this IQueryable<TicketType> ticketTypes);
}
