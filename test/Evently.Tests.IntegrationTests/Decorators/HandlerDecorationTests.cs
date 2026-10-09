using System.Reflection;
using AwesomeAssertions;
using Evently.Shared.Application.Messaging;
using Evently.Tests.IntegrationTests.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Tests.IntegrationTests.Decorators;

[Collection(nameof(IntegrationTestCollection))]
public class HandlerDecorationTests
{
    private static readonly Type[] HandlerTypes =
        [typeof(ICommandHandler<>), typeof(ICommandHandler<,>), typeof(IQueryHandler<,>)];

    private readonly IntegrationTestWebAppFactory _factory;

    public HandlerDecorationTests(IntegrationTestWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public void AllHandlers_Should_BeWrappedByDecorators()
    {
        AssertDecorated(
            _factory.Services,
            Evently.Modules.Events.Application.AssemblyReference.Assembly,
            Evently.Modules.Users.Application.AssemblyReference.Assembly,
            Evently.Modules.Attendance.Application.AssemblyReference.Assembly);

        AssertDecorated(
            _factory.TicketingApi.Services,
            Evently.Modules.Ticketing.Application.AssemblyReference.Assembly);
    }

    private static void AssertDecorated(IServiceProvider services, params Assembly[] assemblies)
    {
        using IServiceScope scope = services.CreateScope();

        Type[] handlerInterfaces = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type is { IsAbstract: false, IsInterface: false })
            .SelectMany(type => type.GetInterfaces())
            .Where(i => i.IsGenericType && HandlerTypes.Contains(i.GetGenericTypeDefinition()))
            .Distinct()
            .ToArray();

        handlerInterfaces.Should().NotBeEmpty();

        foreach (Type handlerInterface in handlerInterfaces)
        {
            object handler = scope.ServiceProvider.GetRequiredService(handlerInterface);

            // decorators are internal: match by name; outermost = ExceptionHandlingDecorator
            handler.GetType().FullName.Should().StartWith(
                "Evently.Shared.Application.Behaviours.ExceptionHandlingDecorator+",
                $"{handlerInterface} must go through the decorator chain");
        }
    }
}
