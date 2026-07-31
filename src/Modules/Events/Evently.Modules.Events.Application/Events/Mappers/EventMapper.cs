using Evently.Modules.Events.Application.Events.Queries.ViewModels;
using Evently.Modules.Events.Domain.Events;
using Riok.Mapperly.Abstractions;

namespace Evently.Modules.Events.Application.Events.Mappers;

[Mapper]
public static partial class EventMapper
{
    // The decorator lets Mapperly map properties ignoring non existent properties on target that do exist on source
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial EventViewModel ToViewModel(this Event @event);
    public static partial IQueryable<EventViewModel> ProjectToViewModel(this IQueryable<Event> events);
}
