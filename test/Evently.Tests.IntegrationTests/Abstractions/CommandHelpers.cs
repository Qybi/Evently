using AwesomeAssertions;
using Bogus;
using Evently.Modules.Ticketing.Application.Events.Commands.CreateEvent;
using Evently.Shared.Domain;
using MediatR;

namespace Evently.Tests.IntegrationTests.Abstractions;

internal static class CommandHelpers
{
    internal static async Task CreateEventAsync(
        this ISender sender,
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

        Result result = await sender.Send(new CreateEventCommand(
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
