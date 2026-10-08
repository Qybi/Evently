using AwesomeAssertions;
using Evently.Modules.Events.Application.Events.Queries.GetEvent;
using Evently.Modules.Events.Application.Events.Queries.ViewModels;
using Evently.Modules.Events.Domain.Events.Errors;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Events.IntegrationTests.Events;

public class GetEventTests : BaseIntegrationTest
{
    public GetEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange
        var query = new GetEventQuery(Guid.NewGuid());

        // Act
        Result<EventViewModel> result = await SendQuery<GetEventQuery, EventViewModel>(query);

        // Assert
        result.Error.Should().Be(EventErrors.NotFound(query.EventId));
    }

    [Fact]
    public async Task Should_ReturnEvent_WhenEventExists()
    {
        // Arrange
        await CleanDatabaseAsync();

        Guid categoryId = await CreateCategoryAsync(Faker.Music.Genre());

        Guid eventId = await CreateEventAsync(categoryId);

        var query = new GetEventQuery(eventId);

        // Act
        Result<EventViewModel> result = await SendQuery<GetEventQuery, EventViewModel>(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
