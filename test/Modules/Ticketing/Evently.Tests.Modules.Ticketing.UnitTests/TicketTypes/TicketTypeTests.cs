using AwesomeAssertions;
using Evently.Modules.Ticketing.Domain.TicketTypes;
using Evently.Modules.Ticketing.Domain.TicketTypes.DomainEvents;
using Evently.Modules.Ticketing.Domain.TicketTypes.Errors;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Ticketing.UnitTests.Abstractions;

namespace Evently.Tests.Modules.Ticketing.UnitTests.TicketTypes;

public class TicketTypeTests : BaseTest
{
    [Fact]
    public void UpdateQuantity_ShouldReturnFailure_WhenNotEnoughQuantity()
    {
        //Arrange
        decimal quantity = Faker.Random.Int(1, 100);

        var ticketType = TicketType.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Random.Decimal(),
            Faker.Finance.Currency().Code,
            quantity);

        //Act
        Result result = ticketType.UpdateQuantity(quantity + 1);

        //Assert
        result.Error.Should().Be(TicketTypeErrors.NotEnoughQuantity(quantity));
    }

    [Fact]
    public void UpdateQuantity_ShouldRaiseDomainEvent_WhenTicketTypeIsSoldOut()
    {
        //Arrange
        decimal quantity = Faker.Random.Int(1, 100);

        var ticketType = TicketType.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Random.Decimal(),
            Faker.Finance.Currency().Code,
            quantity);

        //Act
        ticketType.UpdateQuantity(quantity);

        //Assert
        TicketTypeSoldOutDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketTypeSoldOutDomainEvent>(ticketType);

        domainEvent.TicketTypeId.Should().Be(ticketType.Id);
    }
}
