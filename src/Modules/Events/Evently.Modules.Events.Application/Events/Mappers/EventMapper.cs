using Evently.Modules.Events.Application.Events.DTO;
using Evently.Modules.Events.Domain.Events;
using Riok.Mapperly.Abstractions;

namespace Evently.Modules.Events.Application.Events.Mappers;

[Mapper]
public static partial class EventMapper
{
    // The decorator lets Mapperly map properties ignoring non existent properties on target that do exist on source
    //[MapperRequiredMapping(RequiredMappingStrategy.Target)]

    [MapperIgnoreSource(nameof(Event.Status))]
    public static partial EventResponseDto ToResponseDto(this Event @event);
    public static partial IQueryable<EventResponseDto> ProjectToDto(this IQueryable<Event> events);
}
