using Evently.Modules.Attendance.Application.EventStatistics.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Attendance.Application.EventStatistics.Queries.GetEventStatistics;

public sealed record GetEventStatisticsQuery(Guid EventId) : IQuery<GetEventStatisticsViewModel>;
