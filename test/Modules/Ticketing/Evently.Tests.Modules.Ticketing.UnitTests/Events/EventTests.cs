using AwesomeAssertions;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Events.DomainEvents;
using Evently.Tests.Modules.Ticketing.UnitTests.Abstractions;

namespace Evently.Tests.Modules.Ticketing.UnitTests.Events;

public class EventTests : BaseTest
{
    [Fact]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventRescheduled()
    {
        //Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        //Act
        @event.Reschedule(startsAtUtc.AddDays(1), startsAtUtc.AddDays(2));

        //Assert
        EventRescheduledDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventRescheduledDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventCanceled()
    {
        //Arrange
        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        //Act
        @event.Cancel();

        //Assert
        EventCanceledDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventCanceledDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Cancel_ShouldNotRaiseDomainEvent_WhenEventAlreadyCanceled()
    {
        //Arrange
        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        @event.Cancel();
        @event.ClearDomainEvents();

        //Act
        @event.Cancel();

        //Assert
        @event.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void PaymentsRefunded_ShouldRaiseDomainEvent_WhenPaymentsRefunded()
    {
        //Arrange
        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        //Act
        @event.PaymentsRefunded();

        //Assert
        EventPaymentsRefundedDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventPaymentsRefundedDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void TicketsArchived_ShouldRaiseDomainEvent_WhenTicketsArchived()
    {
        //Arrange
        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        //Act
        @event.TicketsArchived();

        //Assert
        EventTicketsArchivedDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventTicketsArchivedDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }
}
