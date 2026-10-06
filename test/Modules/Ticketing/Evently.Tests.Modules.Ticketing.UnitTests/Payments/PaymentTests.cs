using AwesomeAssertions;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments;
using Evently.Modules.Ticketing.Domain.Payments.DomainEvents;
using Evently.Modules.Ticketing.Domain.Payments.Errors;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Ticketing.UnitTests.Abstractions;

namespace Evently.Tests.Modules.Ticketing.UnitTests.Payments;

public class PaymentTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenPaymentCreated()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        //Act
        var payment = Payment.Create(
            order,
            Guid.NewGuid(),
            Faker.Random.Decimal(1, 100),
            Faker.Finance.Currency().Code);

        //Assert
        PaymentCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<PaymentCreatedDomainEvent>(payment);

        domainEvent.PaymentId.Should().Be(payment.Id);
    }

    [Fact]
    public void Refund_ShouldRaiseDomainEvent_WhenPaymentRefunded()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        decimal amount = Faker.Random.Decimal(1, 100);

        var payment = Payment.Create(
            order,
            Guid.NewGuid(),
            amount,
            Faker.Finance.Currency().Code);

        //Act
        payment.Refund(amount);

        //Assert
        PaymentRefundedDomainEvent domainEvent =
            AssertDomainEventWasPublished<PaymentRefundedDomainEvent>(payment);

        domainEvent.PaymentId.Should().Be(payment.Id);
    }

    [Fact]
    public void Refund_ShouldRaiseDomainEvent_WhenPaymentPartiallyRefunded()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        decimal amount = Faker.Random.Decimal(1, 100);

        var payment = Payment.Create(
            order,
            Guid.NewGuid(),
            amount,
            Faker.Finance.Currency().Code);

        //Act
        payment.Refund(amount / 2);

        //Assert
        PaymentPartiallyRefundedDomainEvent domainEvent =
            AssertDomainEventWasPublished<PaymentPartiallyRefundedDomainEvent>(payment);

        domainEvent.PaymentId.Should().Be(payment.Id);
    }

    [Fact]
    public void Refund_ShouldReturnFailure_WhenPaymentAlreadyRefunded()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        decimal amount = Faker.Random.Decimal(1, 100);

        var payment = Payment.Create(
            order,
            Guid.NewGuid(),
            amount,
            Faker.Finance.Currency().Code);

        payment.Refund(amount);

        //Act
        Result result = payment.Refund(amount);

        //Assert
        result.Error.Should().Be(PaymentErrors.AlreadyRefunded);
    }

    [Fact]
    public void Refund_ShouldReturnFailure_WhenNotEnoughFunds()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        decimal amount = Faker.Random.Decimal(1, 100);

        var payment = Payment.Create(
            order,
            Guid.NewGuid(),
            amount,
            Faker.Finance.Currency().Code);

        //Act
        Result result = payment.Refund(amount + 1);

        //Assert
        result.Error.Should().Be(PaymentErrors.NotEnoughFunds);
    }
}
