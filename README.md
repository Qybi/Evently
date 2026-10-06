# Qybi's Evently

This is a personal flavoured version of Milan's Jovanovic Modular Monolith application course.
My current version runs on the following stack and differences:

- .NET 10.0
- MediatR -> then future Wolverine migration
- FluentValidation
- Mapperly
- CQRS with EF Core instead of Hybrid Dapper/EF Core
- PostgreSQL

## Migrations

cd into src\API\Evently.Api

```powershell
dotnet ef migrations add MIGRATION_NAME -c DB_CONTEXT -o Database\Migrations -p ..\..\Modules\<module>\Evently.Modules.<module>.Infrastructure\Evently.Modules.<module>.Infrastructure.csproj
```