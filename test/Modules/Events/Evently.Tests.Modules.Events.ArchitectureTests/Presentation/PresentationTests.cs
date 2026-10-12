using Evently.Shared.Application.EventBus;
using Evently.Tests.Modules.Events.ArchitectureTests.Abstractions;
using NetArchTest.Rules;

namespace Evently.Tests.Modules.Events.ArchitectureTests.Presentation;

public class PresentationTests : BaseTest
{
    [Fact]
    public void IntegrationEventHandler_Should_BeSealed()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IIntegrationEventHandler<>))
            .Or()
            .Inherit(typeof(IntegrationEventHandler<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void IntegrationEventHandler_ShouldHave_NameEndingWith_IntegrationEventHandler()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IIntegrationEventHandler<>))
            .Or()
            .Inherit(typeof(IntegrationEventHandler<>))
            .Should()
            .HaveNameEndingWith("IntegrationEventHandler")
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void RpcConsumer_Should_BeSealed()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IRpcConsumer<,>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void RpcConsumer_ShouldHave_NameEndingWith_RequestConsumer()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IRpcConsumer<,>))
            .Should()
            .HaveNameEndingWith("RequestConsumer")
            .GetResult()
            .ShouldBeSuccessful();
    }
}
