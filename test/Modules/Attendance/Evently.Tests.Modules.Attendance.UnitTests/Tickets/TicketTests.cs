using AwesomeAssertions;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.Domain.Tickets.DomainEvents;
using Evently.Tests.Modules.Attendance.UnitTests.Abstractions;

namespace Evently.Tests.Modules.Attendance.UnitTests.Tickets;

public class TicketTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenTicketCreated()
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

        //Act
        var ticket = Ticket.Create(Guid.NewGuid(), attendee, @event, Faker.Random.String());

        //Assert
        TicketCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketCreatedDomainEvent>(ticket);

        domainEvent.TicketId.Should().Be(ticket.Id);
    }
}
