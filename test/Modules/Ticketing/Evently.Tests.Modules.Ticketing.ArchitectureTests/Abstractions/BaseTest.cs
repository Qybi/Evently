using System.Reflection;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Infrastructure;

namespace Evently.Tests.Modules.Ticketing.ArchitectureTests.Abstractions;

#pragma warning disable CA1515 // Consider making public types internal
public abstract class BaseTest
#pragma warning restore CA1515 // Consider making public types internal
{
    protected static readonly Assembly ApplicationAssembly = typeof(Evently.Modules.Ticketing.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Customer).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(TicketingModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Evently.Modules.Ticketing.Presentation.AssemblyReference).Assembly;
}
