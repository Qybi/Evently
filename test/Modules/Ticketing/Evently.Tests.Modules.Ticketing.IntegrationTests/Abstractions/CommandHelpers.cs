using AwesomeAssertions;
using Bogus;
using Evently.Modules.Ticketing.Application.Customers.Commands.CreateCustomer;
using Evently.Modules.Ticketing.Application.Events.Commands.CreateEvent;
using Evently.Shared.Domain;

namespace Evently.Tests.Modules.Ticketing.IntegrationTests.Abstractions;

public abstract partial class BaseIntegrationTest
{
    protected async Task<Guid> CreateCustomerAsync(Guid customerId)
    {
        var faker = new Faker();
        Result result = await SendCommand(
            new CreateCustomerCommand(
                customerId,
                faker.Internet.Email(),
                faker.Person.FirstName,
                faker.Person.LastName));

        result.IsSuccess.Should().BeTrue();

        return customerId;
    }

    protected async Task CreateEventWithTicketTypeAsync(
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

        Result result = await SendCommand(new CreateEventCommand(
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
