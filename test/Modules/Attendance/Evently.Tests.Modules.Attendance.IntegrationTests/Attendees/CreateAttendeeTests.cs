using AwesomeAssertions;
using Evently.Modules.Attendance.Application.Attendees.Commands.CreateAttendee;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Attendance.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Attendance.IntegrationTests.Attendees;

public class CreateAttendeeTests : BaseIntegrationTest
{
    public CreateAttendeeTests(IntegrationTestWebAppFactory factory)
       : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new CreateAttendeeCommand(
            Guid.NewGuid(),
            string.Empty,
            string.Empty,
            string.Empty);

        // Act
        Result result = await SendCommand(command);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateAttendeeCommand(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act
        Result result = await SendCommand(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
