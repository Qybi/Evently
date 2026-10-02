using AwesomeAssertions;
using Evently.Modules.Events.Application.TicketTypes.Queries.GetTicketType;
using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;
using Evently.Modules.Events.Domain.TicketTypes.Errors;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Events.IntegrationTests.TicketTypes;

public class GetTicketTypeTests : BaseIntegrationTest
{
    public GetTicketTypeTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
    {
        // Arrange
        var query = new GetTicketTypeQuery(Guid.NewGuid());

        // Act
        Result<TicketTypeViewModel> result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(TicketTypeErrors.NotFound(query.TicketTypeId));
    }

    [Fact]
    public async Task Should_ReturnTicketType_WhenTicketTypeExists()
    {
        // Arrange
        await CleanDatabaseAsync();

        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);

        Guid ticketTypeId = await Sender.CreateTicketTypeAsync(eventId);

        var query = new GetTicketTypeQuery(ticketTypeId);

        // Act
        Result<TicketTypeViewModel> result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
