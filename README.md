
## Migrations

cd into src\API\Evently.Api

```powershell
dotnet ef migrations add MIGRATION_NAME -c DB_CONTEXT -o Database\Migrations -p ..\..\Modules\Events\Evently.Modules.Events.Infrastructure\Evently.Modules.Events.Infrastructure.csproj
```