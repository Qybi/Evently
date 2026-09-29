using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Attendees.DomainEvents;
using Evently.Shared.Application.Messaging;
using EventStatisticsEntity = Evently.Modules.Attendance.Domain.Events.EventStatistics;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class AttendeeCheckedInDomainEventHandler(
    IEventStatisticsRepository eventStatisticsRepository,
    IEventStatisticsQueries eventStatisticsQueries,
    IUnitOfWork unitOfWork)
    : DomainEventHandler<AttendeeCheckedInDomainEvent>
{
    public override async Task Handle(
        AttendeeCheckedInDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        EventStatisticsEntity? eventStatistics = await eventStatisticsRepository.GetAsync(
            domainEvent.EventId,
            cancellationToken);

        if (eventStatistics is null)
        {
            return;
        }

        int attendeesCheckedIn = await eventStatisticsQueries.CountAttendeesCheckedInAsync(
            domainEvent.EventId,
            cancellationToken);

        eventStatistics.UpdateAttendeesCheckedIn(attendeesCheckedIn);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
