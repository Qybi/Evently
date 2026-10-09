using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Evently.Modules.Users.Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Tests.Modules.Users.IntegrationTests.Abstraction;

/// <summary>
/// Webapplication factory creates 
/// </summary>
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Services from test/compose/docker-compose.tests.yml, started as one Docker Compose project
    // named evently-tests-users-<random>, so Docker Desktop shows the containers grouped and named
    private readonly ComposeContainer _composeContainer = new ComposeBuilder("docker:29.8.1-cli")
        .WithComposeFile(Path.Combine("compose", "docker-compose.tests.yml"))
        .WithProjectNamePrefix("evently-tests-users")
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

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:Database", $"Host={_composeContainer.GetServiceHost("database", 5432)};Port={_composeContainer.GetServicePort("database", 5432)};Database=evently;Username=postgres;Password=postgres");
        Environment.SetEnvironmentVariable("ConnectionStrings:Cache", $"{_composeContainer.GetServiceHost("cache", 6379)}:{_composeContainer.GetServicePort("cache", 6379)}");
        Environment.SetEnvironmentVariable("ConnectionStrings:Queue", $"amqp://guest:guest@{_composeContainer.GetServiceHost("queue", 5672)}:{_composeContainer.GetServicePort("queue", 5672)}/");

        string keyCloakAddress = $"http://{_composeContainer.GetServiceHost("identity", 8080)}:{_composeContainer.GetServicePort("identity", 8080)}/";
        string keyCloakRealmUrl = $"{keyCloakAddress}realms/Evently";

        Environment.SetEnvironmentVariable("Authentication:MetadataAddress", $"{keyCloakRealmUrl}/.well-known/openid-configuration");
        Environment.SetEnvironmentVariable("Authentication:TokenValidationParameters:ValidIssuer", keyCloakRealmUrl);

        builder.ConfigureTestServices(services =>
        {
            services.Configure<KeyCloakOptions>(o =>
            {
                o.AdminUrl = $"{keyCloakAddress}admin/realms/Evently/";
                o.TokenUrl = $"{keyCloakRealmUrl}/protocol/openid-connect/token";
            });
        });
    }

    public async Task InitializeAsync()
    {
        _stackLock = await AcquireStackLockAsync();

        await _composeContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        try
        {
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
