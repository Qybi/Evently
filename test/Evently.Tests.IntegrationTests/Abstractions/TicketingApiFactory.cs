extern alias TicketingApi;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Evently.Tests.IntegrationTests.Abstractions;

// Second host for the Ticketing module. It reads the connection strings from the environment variables
// set by IntegrationTestWebAppFactory, so it must be built after that host.
public sealed class TicketingApiFactory : WebApplicationFactory<TicketingApi::Program>;
