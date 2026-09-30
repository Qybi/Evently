using AwesomeAssertions;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Tickets;
using Evently.Modules.Ticketing.Domain.Tickets.DomainEvents;
using Evently.Modules.Ticketing.Domain.TicketTypes;
using Evently.Tests.Modules.Ticketing.UnitTests.Abstractions;

namespace Evently.Tests.Modules.Ticketing.UnitTests.Tickets;

public class TicketTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenTicketCreated()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        var ticketType = TicketType.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Random.Decimal(),
            Faker.Finance.Currency().Code,
            Faker.Random.Decimal());

        //Act
        var ticket = Ticket.Create(order, ticketType);

        //Assert
        TicketCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketCreatedDomainEvent>(ticket);

        domainEvent.TicketId.Should().Be(ticket.Id);
    }

    [Fact]
    public void Archive_ShouldRaiseDomainEvent_WhenTicketArchived()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        var ticketType = TicketType.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Random.Decimal(),
            Faker.Finance.Currency().Code,
            Faker.Random.Decimal());

        var ticket = Ticket.Create(order, ticketType);

        //Act
        ticket.Archive();

        //Assert
        TicketArchivedDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketArchivedDomainEvent>(ticket);

        domainEvent.TicketId.Should().Be(ticket.Id);
    }

    [Fact]
    public void Archive_ShouldNotRaiseDomainEvent_WhenTicketAlreadyArchived()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        var ticketType = TicketType.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Random.Decimal(),
            Faker.Finance.Currency().Code,
            Faker.Random.Decimal());

        var ticket = Ticket.Create(order, ticketType);

        ticket.Archive();
        ticket.ClearDomainEvents();

        //Act
        ticket.Archive();

        //Assert
        ticket.DomainEvents.Should().BeEmpty();
    }
}
