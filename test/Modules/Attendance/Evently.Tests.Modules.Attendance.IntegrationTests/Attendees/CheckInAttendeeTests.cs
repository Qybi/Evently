using AwesomeAssertions;
using Evently.Modules.Attendance.Application.Attendees.Commands.CheckInAttendee;
using Evently.Modules.Attendance.Domain.Attendees.Errors;
using Evently.Modules.Attendance.Domain.Tickets.Errors;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Attendance.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Attendance.IntegrationTests.Attendees;

public class CheckInAttendeeTests : BaseIntegrationTest
{
    public CheckInAttendeeTests(IntegrationTestWebAppFactory factory)
       : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenAttendeeDoesNotExist()
    {
        // Arrange
        var command = new CheckInAttendeeCommand(
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        Result result = await SendCommand(command);

        // Assert
        result.Error.Should().Be(AttendeeErrors.NotFound(command.AttendeeId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketDoesNotExist()
    {
        // Arrange
        Guid attendeeId = await CreateAttendeeAsync(Guid.NewGuid());

        var command = new CheckInAttendeeCommand(
            attendeeId,
            Guid.NewGuid());

        // Act
        Result result = await SendCommand(command);

        // Assert
        result.Error.Should().Be(TicketErrors.NotFound(command.TicketId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenAttendeeCheckedIn()
    {
        //Arrange
        Guid attendeeId = await CreateAttendeeAsync(Guid.NewGuid());
        Guid eventId = await CreateEventAsync(Guid.NewGuid());
        Guid ticketId = await CreateTicketAsync(Guid.NewGuid(), attendeeId, eventId);

        var command = new CheckInAttendeeCommand(
            attendeeId,
            ticketId);

        //Act
        Result result = await SendCommand(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
    }
}
