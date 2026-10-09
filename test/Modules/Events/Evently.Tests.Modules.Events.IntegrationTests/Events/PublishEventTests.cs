using AwesomeAssertions;
using Evently.Modules.Events.Application.Events.Commands.PublishEvent;
using Evently.Modules.Events.Domain.Events.Errors;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Events.IntegrationTests.Events;

public class PublishEventTests : BaseIntegrationTest
{
    public PublishEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        var command = new PublishEventCommand(eventId);

        // Act
        Result result = await SendCommand(command);

        // Assert
        result.Error.Should().Be(EventErrors.NotFound(eventId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotHaveAnyTicketTypes()
    {
        // Arrange
        Guid categoryId = await CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await CreateEventAsync(categoryId);

        var command = new PublishEventCommand(eventId);

        // Act
        Result result = await SendCommand(command);

        //Assert
        result.Error.Should().Be(EventErrors.NoTicketTypesFound);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventIsPublished()
    {
        // Arrange
        Guid categoryId = await CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await CreateEventAsync(categoryId);
        await CreateTicketTypeAsync(eventId);

        var command = new PublishEventCommand(eventId);

        // Act
        Result result = await SendCommand(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
