using AwesomeAssertions;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Events.DomainEvents;
using Evently.Tests.Modules.Attendance.UnitTests.Abstractions;

namespace Evently.Tests.Modules.Attendance.UnitTests.Events;

public class EventTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenEventCreated()
    {
        //Act
        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        //Assert
        EventCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventCreatedDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }
}
