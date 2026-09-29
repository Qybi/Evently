namespace Evently.Modules.Attendance.Domain.Events;

// this class represents a materialized view on data present in the current module. It gets data from the handlers inside Application/EventStatistics/Projections that update and insert
// data on this class correspondant table
public sealed class EventStatistics
{
    private EventStatistics()
    {
    }

    public Guid EventId { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Location { get; private set; }

    public DateTime StartsAtUtc { get; private set; }

    public DateTime? EndsAtUtc { get; private set; }

    public int TicketsSold { get; private set; }

    public int AttendeesCheckedIn { get; private set; }

    public List<string> DuplicateCheckInTickets { get; private set; }

    public List<string> InvalidCheckInTickets { get; private set; }

    public static EventStatistics Create(
        Guid id,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        var @event = new EventStatistics
        {
            EventId = id,
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
            TicketsSold = 0,
            AttendeesCheckedIn = 0,
            DuplicateCheckInTickets = [],
            InvalidCheckInTickets = []
        };

        return @event;
    }

    public void UpdateTicketsSold(int ticketsSold)
    {
        TicketsSold = ticketsSold;
    }

    public void UpdateAttendeesCheckedIn(int attendeesCheckedIn)
    {
        AttendeesCheckedIn = attendeesCheckedIn;
    }

    public void AddDuplicateCheckInTicket(string ticketCode)
    {
        DuplicateCheckInTickets.Add(ticketCode);
    }

    public void AddInvalidCheckInTicket(string ticketCode)
    {
        InvalidCheckInTickets.Add(ticketCode);
    }
}
