using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Tickets.DomainEvents;
using Evently.Shared.Application.Messaging;
using EventStatisticsEntity = Evently.Modules.Attendance.Domain.Events.EventStatistics;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class TicketCreatedDomainEventHandler(
    IEventStatisticsRepository eventStatisticsRepository,
    IEventStatisticsQueries eventStatisticsQueries,
    IUnitOfWork unitOfWork)
    : DomainEventHandler<TicketCreatedDomainEvent>
{
    public override async Task Handle(
        TicketCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        EventStatisticsEntity? eventStatistics = await eventStatisticsRepository.GetAsync(
            domainEvent.EventId,
            cancellationToken);

        if (eventStatistics is null)
        {
            return;
        }

        int ticketsSold = await eventStatisticsQueries.CountTicketsSoldAsync(domainEvent.EventId, cancellationToken);

        eventStatistics.UpdateTicketsSold(ticketsSold);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
