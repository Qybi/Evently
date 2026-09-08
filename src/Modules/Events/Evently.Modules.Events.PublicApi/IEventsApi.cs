namespace Evently.Modules.Events.PublicApi;

public interface IEventsApi
{
    Task<TicketTypePublicApiResponse?> GetTicketTypeAsync(Guid ticketTypeId, CancellationToken cancellationToken);  
}
