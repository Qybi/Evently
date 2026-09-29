using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Attendees.DomainEvents;
using Evently.Shared.Application.Messaging;
using EventStatisticsEntity = Evently.Modules.Attendance.Domain.Events.EventStatistics;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class InvalidCheckInAttemptedDomainEventHandler(
    IEventStatisticsRepository eventStatisticsRepository,
    IUnitOfWork unitOfWork)
    : DomainEventHandler<InvalidCheckInAttemptedDomainEvent>
{
    public override async Task Handle(
        InvalidCheckInAttemptedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        EventStatisticsEntity? eventStatistics = await eventStatisticsRepository.GetAsync(
            domainEvent.EventId,
            cancellationToken);

        if (eventStatistics is null)
        {
            return;
        }

        eventStatistics.AddInvalidCheckInTicket(domainEvent.TicketCode);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
