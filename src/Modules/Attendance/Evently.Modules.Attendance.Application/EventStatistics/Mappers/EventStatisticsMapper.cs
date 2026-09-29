using Evently.Modules.Attendance.Application.EventStatistics.ViewModels;
using Riok.Mapperly.Abstractions;
using EventStatisticsEntity = Evently.Modules.Attendance.Domain.Events.EventStatistics;

namespace Evently.Modules.Attendance.Application.EventStatistics.Mappers;

[Mapper]
public static partial class EventStatisticsMapper
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial GetEventStatisticsViewModel ToViewModel(this EventStatisticsEntity eventStatistics);
    public static partial IQueryable<GetEventStatisticsViewModel> ProjectToViewModel(this IQueryable<EventStatisticsEntity> eventStatistics);
}
