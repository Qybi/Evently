using AwesomeAssertions;
using Evently.Modules.Events.Application.TicketTypes.Commands.UpdateTicketTypePrice;
using Evently.Modules.Events.Domain.TicketTypes.Errors;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Events.IntegrationTests.TicketTypes;

public class UpdateTicketTypeTests : BaseIntegrationTest
{
    public UpdateTicketTypeTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
    {
        // Arrange
        var command = new UpdateTicketTypePriceCommand(Guid.NewGuid(), Faker.Random.Decimal());

        // Act
        Result result = await SendCommand(command);

        // Assert
        result.Error.Should().Be(TicketTypeErrors.NotFound(command.TicketTypeId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenTicketTypeExists()
    {
        // Arrange
        Guid categoryId = await CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await CreateEventAsync(categoryId);
        Guid ticketTypeId = await CreateTicketTypeAsync(eventId);

        var command = new UpdateTicketTypePriceCommand(ticketTypeId, Faker.Random.Decimal());

        // Act
        Result result = await SendCommand(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
