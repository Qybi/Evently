using Evently.Modules.Events.Application.Events.DTO;
using Evently.Modules.Events.Domain.Events;
using Riok.Mapperly.Abstractions;

namespace Evently.Modules.Events.Application.Events.Mappers;

[Mapper]
public static partial class EventMapper
{
    // The decorator lets Mapperly map properties ignoring non existent properties on target that do exist on source

    //[MapperIgnoreSource(nameof(Event.Status))]
    //[MapperIgnoreSource(nameof(Event.DomainEvents))]
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial EventResponseDto ToResponseDto(this Event @event);
    public static partial IQueryable<EventResponseDto> ProjectToDto(this IQueryable<Event> events);
}
