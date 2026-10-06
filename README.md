# Qybi's Evently

This is a personal flavoured version of Milan's Jovanovic Modular Monolith application course.

## Differences from the course reference

Compared against the course's final reference solution.

### Stack and build

- .NET 10 (vs .NET 8) with `Evently.slnx` instead of `Evently.sln`.
- **No Dapper**: reads go through EF Core `IXxxQueries`, writes through `IXxxRepository` (interfaces in Application, implementations in Infrastructure `Queries/` and `Repositories/`).
- Central package management via `Directory.Packages.props`.
- Different project structure and naming conventions.
- Mapperly for read-model projections; read models are `*ViewModel` instead of `*Response`.
- AwesomeAssertions instead of FluentAssertions.
- Quartz 4 instead of 3, MediatR 14, FluentValidation 12, Testcontainers 4, YARP 2.3, SonarAnalyzer 10, HealthChecks 9.
- No `Ulid` package: `Guid.CreateVersion7()` for domain event ids.
- no Swagger UI.
- GitHub Actions `build.yml` (manual or `r-x.y.z` tag), 

### Project layout

- Feature folders split into `Commands/`, `Queries/`, `Mappers/`, `ViewModels/` (Application), `DomainEvents/`, `Errors/` (Domain) and `Database/Configurations/` (Infrastructure);
- Some modules split Presentation into `Endpoints/` and `IntegrationEventHandlers/` where needed.
- Repository interfaces live in Application instead of Domain.
- Tests moved out of `src/Modules` into `test/Modules/<Module>/Evently.Tests.Modules.<Module>.*`; cross-module tests are `test/Evently.Tests.Architecture` and `test/Evently.Tests.IntegrationTests`.

### Hosts and infrastructure

- Health checks live in `HealthChecksExtensions` with a dedicated RabbitMQ connection; Keycloak is checked on the management port (`9000/health/ready`).
- `modules.*.Development.json` files are required; outbox/inbox interval is 15 s (reference: 5 s).
- docker-compose pins image versions (postgres 18, redis 8, keycloak 26.7, rabbitmq 4, jaeger 1.76), adds healthchecks with `depends_on: service_healthy` and uses different host ports for Seq, Jaeger and RabbitMQ.
- Dockerfiles build from the repo root, copy the props files and run as `$APP_UID`.

## Helper commands

### Migrations

cd into src\API\Evently.Api

```powershell
dotnet ef migrations add MIGRATION_NAME -c DB_CONTEXT -o Database\Migrations -p ..\..\Modules\<module>\Evently.Modules.<module>.Infrastructure\Evently.Modules.<module>.Infrastructure.csproj
```