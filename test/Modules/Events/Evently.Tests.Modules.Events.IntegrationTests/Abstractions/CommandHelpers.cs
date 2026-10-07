using Bogus;
using Evently.Modules.Events.Application.Categories.Commands.CreateCategory;
using Evently.Modules.Events.Application.Events.Commands.CreateEvent;
using Evently.Modules.Events.Application.TicketTypes.Commands.CreateTicketType;
using Evently.Shared.Domain;

namespace Evently.Tests.Modules.Events.IntegrationTests.Abstractions;

public abstract partial class BaseIntegrationTest
{
    protected async Task<Guid> CreateCategoryAsync(string name)
    {
        Result<Guid> result = await SendCommand<CreateCategoryCommand, Guid>(new CreateCategoryCommand(name));

        return result.Value;
    }

    protected async Task<Guid> CreateEventAsync(
        Guid categoryId,
        DateTime? startsAtUtc = null)
    {
        var faker = new Faker();
        Result<Guid> result = await SendCommand<CreateEventCommand, Guid>(
            new CreateEventCommand(
                categoryId,
                faker.Music.Genre(),
                faker.Music.Genre(),
                faker.Address.StreetAddress(),
                startsAtUtc ?? DateTime.UtcNow.AddMinutes(10),
                null));

        return result.Value;
    }

    protected async Task<Guid> CreateTicketTypeAsync(Guid eventId)
    {
        var faker = new Faker();
        Result<Guid> result = await SendCommand<CreateTicketTypeCommand, Guid>(
            new CreateTicketTypeCommand(
                eventId,
                faker.Commerce.ProductName(),
                faker.Random.Decimal(),
                "USD",
                faker.Random.Decimal()));

        return result.Value;
    }
}
