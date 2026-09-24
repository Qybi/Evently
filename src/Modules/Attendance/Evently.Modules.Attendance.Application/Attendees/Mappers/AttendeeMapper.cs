using Evently.Modules.Attendance.Application.Attendees.Queries.ViewModels;
using Evently.Modules.Attendance.Domain.Attendees;
using Riok.Mapperly.Abstractions;

namespace Evently.Modules.Attendance.Application.Attendees.Mappers;

[Mapper]
public static partial class AttendeeMapper
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial AttendeeViewModel ToViewModel(this Attendee attendee);
    public static partial IQueryable<AttendeeViewModel> ProjectToViewModel(this IQueryable<Attendee> attendees);
}
