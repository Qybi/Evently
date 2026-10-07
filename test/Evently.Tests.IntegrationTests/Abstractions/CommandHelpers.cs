using AwesomeAssertions;
using Bogus;
using Evently.Modules.Ticketing.Application.Events.Commands.CreateEvent;
using Evently.Shared.Domain;

namespace Evently.Tests.IntegrationTests.Abstractions;

public abstract partial class BaseIntegrationTest
{
    protected async Task CreateEventAsync(
        Guid eventId,
        Guid ticketTypeId,
        decimal quantity)
    {
        var faker = new Faker();

        var ticketType = new CreateEventCommand.TicketTypeRequest(
            ticketTypeId,
            eventId,
            faker.Music.Genre(),
            faker.Random.Decimal(),
            "USD",
            quantity);

        Result result = await SendTicketingCommand(new CreateEventCommand(
            eventId,
            faker.Music.Genre(),
            faker.Music.Genre(),
            faker.Address.FullAddress(),
            DateTime.UtcNow,
            null,
            [ticketType]));

        result.IsSuccess.Should().BeTrue();
    }
}
