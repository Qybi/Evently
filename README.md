# Qybi's Evently

This is a personal flavoured version of Milan's Jovanovic Modular Monolith application course.
My current version runs on the following stack:


## Migrations

cd into src\API\Evently.Api

```powershell
dotnet ef migrations add MIGRATION_NAME -c DB_CONTEXT -o Database\Migrations -p ..\..\Modules\Events\Evently.Modules.Events.Infrastructure\Evently.Modules.Events.Infrastructure.csproj
```