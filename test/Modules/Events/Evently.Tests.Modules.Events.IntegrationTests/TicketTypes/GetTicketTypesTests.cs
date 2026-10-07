using AwesomeAssertions;
using Evently.Modules.Events.Application.TicketTypes.Queries.GetEventTicketTypes;
using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Events.IntegrationTests.TicketTypes;

public class GetTicketTypesTests : BaseIntegrationTest
{
    public GetTicketTypesTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypesDoNotExist()
    {
        // Arrange
        await CleanDatabaseAsync();

        var query = new GetEventTicketTypesQuery(Guid.NewGuid());

        // Act
        Result<IReadOnlyCollection<TicketTypeViewModel>> result = await SendQuery<GetEventTicketTypesQuery, IReadOnlyCollection<TicketTypeViewModel>>(query);

        // Assert
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_ReturnTicketTypes_WhenTicketTypesExists()
    {
        // Arrange
        await CleanDatabaseAsync();

        Guid categoryId = await CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await CreateEventAsync(categoryId);

        await CreateTicketTypeAsync(eventId);
        await CreateTicketTypeAsync(eventId);

        var query = new GetEventTicketTypesQuery(eventId);

        // Act
        Result<IReadOnlyCollection<TicketTypeViewModel>> result = await SendQuery<GetEventTicketTypesQuery, IReadOnlyCollection<TicketTypeViewModel>>(query);

        // Assert
        result.Value.Should().HaveCount(2);
    }
}
