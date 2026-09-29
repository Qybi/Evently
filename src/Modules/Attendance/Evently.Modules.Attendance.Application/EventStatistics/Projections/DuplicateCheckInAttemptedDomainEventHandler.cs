using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Attendees.DomainEvents;
using Evently.Shared.Application.Messaging;
using EventStatisticsEntity = Evently.Modules.Attendance.Domain.Events.EventStatistics;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class DuplicateCheckInAttemptedDomainEventHandler(
    IEventStatisticsRepository eventStatisticsRepository,
    IUnitOfWork unitOfWork)
    : DomainEventHandler<DuplicateCheckInAttemptedDomainEvent>
{
    public override async Task Handle(
        DuplicateCheckInAttemptedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        EventStatisticsEntity? eventStatistics = await eventStatisticsRepository.GetAsync(
            domainEvent.EventId,
            cancellationToken);

        if (eventStatistics is null)
        {
            return;
        }

        eventStatistics.AddDuplicateCheckInTicket(domainEvent.TicketCode);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
