using AwesomeAssertions;
using Evently.Modules.Attendance.Application.Events.Commands.CreateEvent;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Attendance.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Attendance.IntegrationTests.Events;

public class CreateEventTests : BaseIntegrationTest
{
    public CreateEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    public static readonly TheoryData<Guid, string, string, string, DateTime, DateTime?> InvalidData = new()
    {
        { Guid.Empty, Faker.Music.Genre(), Faker.Music.Genre(), Faker.Address.StreetAddress(), default, default },
        { Guid.NewGuid(), string.Empty, Faker.Music.Genre(), Faker.Address.StreetAddress(), default, default },
        { Guid.NewGuid(), Faker.Music.Genre(), string.Empty, Faker.Address.StreetAddress(), default, default },
        { Guid.NewGuid(), Faker.Music.Genre(), Faker.Music.Genre(), string.Empty, default, default },
        { Guid.NewGuid(), Faker.Music.Genre(), Faker.Music.Genre(), Faker.Address.StreetAddress(), default, default }
    };

    [Theory]
    [MemberData(nameof(InvalidData))]
    public async Task Should_ReturnFailure_WhenCommandIsInvalid(
        Guid eventId,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        // Arrange
        var command = new CreateEventCommand(eventId, title, description, location, startsAtUtc, endsAtUtc);

        // Act
        Result result = await SendCommand(command);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenCommandIsValid()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        var command = new CreateEventCommand(
            eventId,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow.AddMinutes(10),
            null);

        // Act
        Result result = await SendCommand(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
