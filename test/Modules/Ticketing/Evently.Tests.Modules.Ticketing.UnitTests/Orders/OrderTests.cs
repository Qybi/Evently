using AwesomeAssertions;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Orders.DomainEvents;
using Evently.Modules.Ticketing.Domain.Orders.Errors;
using Evently.Modules.Ticketing.Domain.TicketTypes;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Ticketing.UnitTests.Abstractions;

namespace Evently.Tests.Modules.Ticketing.UnitTests.Orders;

public class OrderTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenOrderCreated()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        //Act
        var order = Order.Create(customer);

        //Assert
        OrderCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<OrderCreatedDomainEvent>(order);

        domainEvent.OrderId.Should().Be(order.Id);
    }

    [Fact]
    public void AddItem_ShouldUpdateTotalPrice_WhenItemAdded()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        decimal quantity = Faker.Random.Int(1, 10);
        decimal price = Faker.Random.Decimal(1, 100);
        string currency = Faker.Finance.Currency().Code;

        var ticketType = TicketType.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Faker.Music.Genre(),
            price,
            currency,
            quantity);

        //Act
        order.AddItem(ticketType, quantity, price, currency);

        //Assert
        order.TotalPrice.Should().Be(quantity * price);
    }

    [Fact]
    public void IssueTickets_ShouldRaiseDomainEvent_WhenTicketsIssued()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        //Act
        order.IssueTickets();

        //Assert
        OrderTicketsIssuedDomainEvent domainEvent =
            AssertDomainEventWasPublished<OrderTicketsIssuedDomainEvent>(order);

        domainEvent.OrderId.Should().Be(order.Id);
    }

    [Fact]
    public void IssueTickets_ShouldReturnFailure_WhenTicketsAlreadyIssued()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        order.IssueTickets();

        //Act
        Result result = order.IssueTickets();

        //Assert
        result.Error.Should().Be(OrderErrors.TicketsAlreadyIssues);
    }
}
