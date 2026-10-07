using Bogus;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Tests.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract partial class BaseIntegrationTest : IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IServiceScope _ticketingScope;
    protected readonly Faker Faker = new();

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _ticketingScope = factory.TicketingApi.Services.CreateScope();
    }

    protected async Task<Result<TResult>> SendCommand<TCommand, TResult>(TCommand command)
        where TCommand : ICommand<TResult>
    {
        ICommandHandler<TCommand, TResult> handler = _scope.ServiceProvider
            .GetRequiredService<ICommandHandler<TCommand, TResult>>();

        return await handler.Handle(command, CancellationToken.None);
    }

    protected async Task<Result> SendCommand<TCommand>(TCommand command)
        where TCommand : ICommand
    {
        ICommandHandler<TCommand> handler = _scope.ServiceProvider
            .GetRequiredService<ICommandHandler<TCommand>>();

        return await handler.Handle(command, CancellationToken.None);
    }

    protected async Task<Result<TResult>> SendQuery<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>
    {
        IQueryHandler<TQuery, TResult> handler = _scope.ServiceProvider
            .GetRequiredService<IQueryHandler<TQuery, TResult>>();

        return await handler.Handle(query, CancellationToken.None);
    }

    protected async Task<Result> SendTicketingCommand<TCommand>(TCommand command)
        where TCommand : ICommand
    {
        ICommandHandler<TCommand> handler = _ticketingScope.ServiceProvider
            .GetRequiredService<ICommandHandler<TCommand>>();

        return await handler.Handle(command, CancellationToken.None);
    }

    protected async Task<Result<TResult>> SendTicketingQuery<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>
    {
        IQueryHandler<TQuery, TResult> handler = _ticketingScope.ServiceProvider
            .GetRequiredService<IQueryHandler<TQuery, TResult>>();

        return await handler.Handle(query, CancellationToken.None);
    }

    public void Dispose()
    {
        _ticketingScope.Dispose();
        _scope.Dispose();
    }
}
