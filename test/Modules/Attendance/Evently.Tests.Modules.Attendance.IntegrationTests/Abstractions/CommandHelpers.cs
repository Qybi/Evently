using AwesomeAssertions;
using Bogus;
using Evently.Modules.Attendance.Application.Attendees.Commands.CreateAttendee;
using Evently.Modules.Attendance.Application.Events.Commands.CreateEvent;
using Evently.Modules.Attendance.Application.Tickets.Commands.CreateTicket;
using Evently.Shared.Domain;

namespace Evently.Tests.Modules.Attendance.IntegrationTests.Abstractions;

public abstract partial class BaseIntegrationTest
{
    protected async Task<Guid> CreateAttendeeAsync(Guid attendeeId)
    {
        var faker = new Faker();
        Result result = await SendCommand(
            new CreateAttendeeCommand(
                attendeeId,
                faker.Internet.Email(),
                faker.Name.FirstName(),
                faker.Name.LastName()));

        result.IsSuccess.Should().BeTrue();

        return attendeeId;
    }

    protected async Task<Guid> CreateTicketAsync(
        Guid ticketId,
        Guid attendeeId,
        Guid eventId)
    {
        Result result = await SendCommand(
            new CreateTicketCommand(
                ticketId,
                attendeeId,
                eventId,
                $"tc_{Guid.CreateVersion7()}"));

        result.IsSuccess.Should().BeTrue();

        return ticketId;
    }

    protected async Task<Guid> CreateEventAsync(Guid eventId)
    {
        var faker = new Faker();
        Result result = await SendCommand(
            new CreateEventCommand(
                eventId,
                faker.Music.Genre(),
                faker.Music.Genre(),
                faker.Address.StreetAddress(),
                DateTime.UtcNow.AddMinutes(10),
                null));

        result.IsSuccess.Should().BeTrue();

        return eventId;
    }
}
