using AwesomeAssertions;
using Evently.Modules.Ticketing.Application.Events.Commands.CreateEvent;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Ticketing.IntegrationTests.Events;

public class CreateEventTests : BaseIntegrationTest
{
    public CreateEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventIsCreated()
    {
        //Arrange
        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();
        decimal quantity = Faker.Random.Decimal();

        var ticketType = new CreateEventCommand.TicketTypeRequest(
            ticketTypeId,
            eventId,
            Faker.Music.Genre(),
            Faker.Random.Decimal(),
            "USD",
            quantity);

        var command = new CreateEventCommand(
            eventId,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.FullAddress(),
            DateTime.UtcNow,
            null,
            [ticketType]);

        //Act
        Result result = await SendCommand(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
    }
}
