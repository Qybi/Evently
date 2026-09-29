using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Events.DomainEvents;
using Evently.Shared.Application.Messaging;
using EventStatisticsEntity = Evently.Modules.Attendance.Domain.Events.EventStatistics;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class EventCreatedDomainEventHandler(
    IEventStatisticsRepository eventStatisticsRepository,
    IUnitOfWork unitOfWork)
    : DomainEventHandler<EventCreatedDomainEvent>
{
    public override async Task Handle(
        EventCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        var eventStatistics = EventStatisticsEntity.Create(
            domainEvent.EventId,
            domainEvent.Title,
            domainEvent.Description,
            domainEvent.Location,
            domainEvent.StartsAtUtc,
            domainEvent.EndsAtUtc);

        eventStatisticsRepository.Insert(eventStatistics);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
