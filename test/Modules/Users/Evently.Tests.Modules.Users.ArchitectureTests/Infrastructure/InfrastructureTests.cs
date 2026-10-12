using Evently.Modules.Users.Infrastructure.Inbox;
using Evently.Tests.Modules.Users.ArchitectureTests.Abstractions;
using NetArchTest.Rules;

namespace Evently.Tests.Modules.Users.ArchitectureTests.Infrastructure;

public class InfrastructureTests : BaseTest
{
    [Fact]
    public void IntegrationEventConsumer_Should_BeSealed()
    {
        Types.InAssembly(InfrastructureAssembly)
            .That()
            .Inherit(typeof(IntegrationEventConsumer<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void IntegrationEventConsumer_ShouldHave_NameEndingWith_IntegrationEventConsumer()
    {
        Types.InAssembly(InfrastructureAssembly)
            .That()
            .Inherit(typeof(IntegrationEventConsumer<>))
            .Should()
            .HaveNameEndingWith("IntegrationEventConsumer")
            .GetResult()
            .ShouldBeSuccessful();
    }
}
