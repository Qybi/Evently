using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Tests.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest : IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IServiceScope _ticketingScope;
    protected readonly ISender Sender;
    protected readonly ISender TicketingSender;
    protected readonly Faker Faker = new();

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        _ticketingScope = factory.TicketingApi.Services.CreateScope();
        TicketingSender = _ticketingScope.ServiceProvider.GetRequiredService<ISender>();
    }

    public void Dispose()
    {
        _ticketingScope.Dispose();
        _scope.Dispose();
    }
}
