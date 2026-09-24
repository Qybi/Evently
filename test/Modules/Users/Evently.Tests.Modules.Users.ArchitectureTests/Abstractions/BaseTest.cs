using System.Reflection;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure;

namespace Evently.Tests.Modules.Users.ArchitectureTests.Abstractions;

#pragma warning disable CA1515 // Consider making public types internal
public abstract class BaseTest
#pragma warning restore CA1515 // Consider making public types internal
{
    protected static readonly Assembly ApplicationAssembly = typeof(Evently.Modules.Users.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(User).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(UsersModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Evently.Modules.Users.Presentation.AssemblyReference).Assembly;
}
