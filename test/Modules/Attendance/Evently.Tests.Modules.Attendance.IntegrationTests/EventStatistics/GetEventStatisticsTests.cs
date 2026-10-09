using AwesomeAssertions;
using Evently.Modules.Attendance.Application.EventStatistics.Queries.GetEventStatistics;
using Evently.Modules.Attendance.Application.EventStatistics.ViewModels;
using Evently.Modules.Attendance.Domain.Events.Errors;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Attendance.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Attendance.IntegrationTests.EventStatistics;

public class GetEventStatisticsTests : BaseIntegrationTest
{
    public GetEventStatisticsTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventStatisticsDoesNotExist()
    {
        // Arrange
        var query = new GetEventStatisticsQuery(Guid.NewGuid());

        // Act
        Result<GetEventStatisticsViewModel> result = await SendQuery<GetEventStatisticsQuery, GetEventStatisticsViewModel>(query);

        // Assert
        result.Error.Should().Be(EventErrors.NotFound(query.EventId));
    }
}
