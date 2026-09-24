using System.Reflection;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Infrastructure;

namespace Evently.Tests.Modules.Events.ArchitectureTests.Abstractions;

#pragma warning disable CA1515 // Consider making public types internal
public abstract class BaseTest
#pragma warning restore CA1515 // Consider making public types internal
{
    protected static readonly Assembly ApplicationAssembly = typeof(Evently.Modules.Events.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Category).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(EventsModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Evently.Modules.Events.Presentation.AssemblyReference).Assembly;
}
