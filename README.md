
## Migrations

cd into src/API/Evently.Api

```powershell
dotnet ef migrations add CreateDatabase -c EventsDbContext -o Database/Migrations -p ..\..\Modules\Events\Evently.Modules.Events.Api\Evently.Modules.Events.Api.csproj
```