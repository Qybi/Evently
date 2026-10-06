using AwesomeAssertions;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Attendees.DomainEvents;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.Domain.Tickets.DomainEvents;
using Evently.Modules.Attendance.Domain.Tickets.Errors;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Attendance.UnitTests.Abstractions;

namespace Evently.Tests.Modules.Attendance.UnitTests.Attendees;

public class AttendeeTests : BaseTest
{
    [Fact]
    public void CheckIn_ShouldReturnFailure_WhenTicketIsNotValid()
    {
        //Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var otherAttendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        var ticket = Ticket.Create(Guid.NewGuid(), otherAttendee, @event, Faker.Random.String());

        //Act
        Result result = attendee.CheckIn(ticket);

        //Assert
        result.Error.Should().Be(TicketErrors.InvalidCheckIn);
    }

    [Fact]
    public void CheckIn_ShouldRaiseDomainEvent_WhenTicketIsNotValid()
    {
        //Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var otherAttendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        var ticket = Ticket.Create(Guid.NewGuid(), otherAttendee, @event, Faker.Random.String());

        //Act
        attendee.CheckIn(ticket);

        //Assert
        InvalidCheckInAttemptedDomainEvent domainEvent =
            AssertDomainEventWasPublished<InvalidCheckInAttemptedDomainEvent>(attendee);

        domainEvent.TicketId.Should().Be(ticket.Id);
    }

    [Fact]
    public void CheckIn_ShouldReturnFailure_WhenTicketAlreadyUsed()
    {
        //Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        var ticket = Ticket.Create(Guid.NewGuid(), attendee, @event, Faker.Random.String());

        attendee.CheckIn(ticket);

        //Act
        Result result = attendee.CheckIn(ticket);

        //Assert
        result.Error.Should().Be(TicketErrors.DuplicateCheckIn);
    }

    [Fact]
    public void CheckIn_ShouldRaiseDomainEvent_WhenTicketAlreadyUsed()
    {
        //Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        var ticket = Ticket.Create(Guid.NewGuid(), attendee, @event, Faker.Random.String());

        attendee.CheckIn(ticket);

        //Act
        attendee.CheckIn(ticket);

        //Assert
        DuplicateCheckInAttemptedDomainEvent domainEvent =
            AssertDomainEventWasPublished<DuplicateCheckInAttemptedDomainEvent>(attendee);

        domainEvent.TicketId.Should().Be(ticket.Id);
    }

    [Fact]
    public void CheckIn_ShouldRaiseDomainEvent_WhenSuccessfullyCheckedIn()
    {
        //Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        var ticket = Ticket.Create(Guid.NewGuid(), attendee, @event, Faker.Random.String());

        //Act
        attendee.CheckIn(ticket);

        //Assert
        AttendeeCheckedInDomainEvent domainEvent =
            AssertDomainEventWasPublished<AttendeeCheckedInDomainEvent>(attendee);

        domainEvent.AttendeeId.Should().Be(attendee.Id);
    }

    [Fact]
    public void CheckIn_ShouldMarkTicketAsUsed_WhenSuccessfullyCheckedIn()
    {
        //Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        var ticket = Ticket.Create(Guid.NewGuid(), attendee, @event, Faker.Random.String());

        //Act
        attendee.CheckIn(ticket);

        //Assert
        TicketUsedDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketUsedDomainEvent>(ticket);

        domainEvent.TicketId.Should().Be(ticket.Id);
    }
}
