using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Evently.Modules.Users.Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Tests.IntegrationTests.Abstractions;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Services from test/compose/docker-compose.tests.yml, started as one Docker Compose project
    // named evently-tests-cross-module-<random>, so Docker Desktop shows the containers grouped and named
    private readonly ComposeContainer _composeContainer = new ComposeBuilder("docker:29.8.1-cli")
        .WithComposeFile(Path.Combine("compose", "docker-compose.tests.yml"))
        .WithProjectNamePrefix("evently-tests-cross-module")
        .WithService("database", "cache", "queue", "identity")
        .WithExposedService("database", 5432, Wait.ForUnixContainer().UntilContainerIsHealthy())
        .WithExposedService("cache", 6379, Wait.ForUnixContainer().UntilContainerIsHealthy())
        .WithExposedService("queue", 5672, Wait.ForUnixContainer().UntilContainerIsHealthy())
        .WithExposedService("identity", 8080, Wait.ForUnixContainer().UntilMessageIsLogged("Listening on: http://0.0.0.0:8080"))
        .WithComposeDownOption("--rmi", "local") // also remove the Keycloak image built for this run
        .Build();

    // One test stack at a time on this machine: every integration test project waits for this lock before starting its containers.
    // A lock file instead of a named Mutex, because a Mutex must be released by the thread that took it and await can resume on another one
    private static readonly string StackLockPath = Path.Combine(Path.GetTempPath(), "evently-integration-tests.lock");

    private FileStream? _stackLock;

    // Outbox and inbox jobs poll every 15 seconds in Development and a cross-module flow chains several of them, so tests poll every 3 seconds.
    // Added as the last configuration source: it overrides the modules.*.json files, which are added after the environment variables
    internal static readonly IReadOnlyDictionary<string, string?> MessagingIntervals = new Dictionary<string, string?>
    {
        ["Events:Outbox:IntervalInSeconds"] = "3",
        ["Events:Inbox:IntervalInSeconds"] = "3",
        ["Users:Outbox:IntervalInSeconds"] = "3",
        ["Users:Inbox:IntervalInSeconds"] = "3",
        ["Attendance:Outbox:IntervalInSeconds"] = "3",
        ["Attendance:Inbox:IntervalInSeconds"] = "3",
        ["Ticketing:Outbox:IntervalInSeconds"] = "3",
        ["Ticketing:Inbox:IntervalInSeconds"] = "3"
    };

    public TicketingApiFactory TicketingApi { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:Database", $"Host={_composeContainer.GetServiceHost("database", 5432)};Port={_composeContainer.GetServicePort("database", 5432)};Database=evently;Username=postgres;Password=postgres");
        Environment.SetEnvironmentVariable("ConnectionStrings:Cache", $"{_composeContainer.GetServiceHost("cache", 6379)}:{_composeContainer.GetServicePort("cache", 6379)}");
        Environment.SetEnvironmentVariable("ConnectionStrings:Queue", $"amqp://guest:guest@{_composeContainer.GetServiceHost("queue", 5672)}:{_composeContainer.GetServicePort("queue", 5672)}/");

        string keycloakAddress = $"http://{_composeContainer.GetServiceHost("identity", 8080)}:{_composeContainer.GetServicePort("identity", 8080)}/";
        string keyCloakRealmUrl = $"{keycloakAddress}realms/Evently";

        Environment.SetEnvironmentVariable(
            "Authentication:MetadataAddress",
            $"{keyCloakRealmUrl}/.well-known/openid-configuration");
        Environment.SetEnvironmentVariable(
            "Authentication:TokenValidationParameters:ValidIssuer",
            keyCloakRealmUrl);

        builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(MessagingIntervals));

        builder.ConfigureTestServices(services =>
        {
            services.Configure<KeyCloakOptions>(o =>
            {
                o.AdminUrl = $"{keycloakAddress}admin/realms/Evently/";
                o.TokenUrl = $"{keyCloakRealmUrl}/protocol/openid-connect/token";
            });
        });
    }

    public async Task InitializeAsync()
    {
        _stackLock = await AcquireStackLockAsync();

        await _composeContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        try
        {
            await TicketingApi.DisposeAsync();

            await _composeContainer.StopAsync();
        }
        finally
        {
            // Released even if stopping fails: Visual Studio can keep the test process alive between runs
            if (_stackLock is not null)
            {
                await _stackLock.DisposeAsync();
            }
        }
    }

    private static async Task<FileStream> AcquireStackLockAsync()
    {
        while (true)
        {
            try
            {
                // FileShare.None: the OS refuses every other open until this stream is disposed or the process exits
                return new FileStream(StackLockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
