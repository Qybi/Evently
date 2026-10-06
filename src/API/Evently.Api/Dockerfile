# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/API/Evently.Api/Evently.Api.csproj", "src/API/Evently.Api/"]
COPY ["src/Modules/Attendance/Evently.Modules.Attendance.Infrastructure/Evently.Modules.Attendance.Infrastructure.csproj", "src/Modules/Attendance/Evently.Modules.Attendance.Infrastructure/"]
COPY ["src/Shared/Evently.Shared.Infrastructure/Evently.Shared.Infrastructure.csproj", "src/Shared/Evently.Shared.Infrastructure/"]
COPY ["src/Shared/Evently.Shared.Application/Evently.Shared.Application.csproj", "src/Shared/Evently.Shared.Application/"]
COPY ["src/Shared/Evently.Shared.Domain/Evently.Shared.Domain.csproj", "src/Shared/Evently.Shared.Domain/"]
COPY ["src/Modules/Attendance/Evently.Modules.Attendance.Application/Evently.Modules.Attendance.Application.csproj", "src/Modules/Attendance/Evently.Modules.Attendance.Application/"]
COPY ["src/Modules/Attendance/Evently.Modules.Attendance.Domain/Evently.Modules.Attendance.Domain.csproj", "src/Modules/Attendance/Evently.Modules.Attendance.Domain/"]
COPY ["src/Modules/Attendance/Evently.Modules.Attendance.Presentation/Evently.Modules.Attendance.Presentation.csproj", "src/Modules/Attendance/Evently.Modules.Attendance.Presentation/"]
COPY ["src/Shared/Evently.Shared.Presentation/Evently.Shared.Presentation.csproj", "src/Shared/Evently.Shared.Presentation/"]
COPY ["src/Modules/Events/Evently.Modules.Events.IntegrationEvents/Evently.Modules.Events.IntegrationEvents.csproj", "src/Modules/Events/Evently.Modules.Events.IntegrationEvents/"]
COPY ["src/Modules/Ticketing/Evently.Modules.Ticketing.IntegrationEvents/Evently.Modules.Ticketing.IntegrationEvents.csproj", "src/Modules/Ticketing/Evently.Modules.Ticketing.IntegrationEvents/"]
COPY ["src/Modules/Users/Evently.Modules.Users.IntegrationEvents/Evently.Modules.Users.IntegrationEvents.csproj", "src/Modules/Users/Evently.Modules.Users.IntegrationEvents/"]
COPY ["src/Modules/Events/Evently.Modules.Events.Infrastructure/Evently.Modules.Events.Infrastructure.csproj", "src/Modules/Events/Evently.Modules.Events.Infrastructure/"]
COPY ["src/Modules/Events/Evently.Modules.Events.Application/Evently.Modules.Events.Application.csproj", "src/Modules/Events/Evently.Modules.Events.Application/"]
COPY ["src/Modules/Events/Evently.Modules.Events.Domain/Evently.Modules.Events.Domain.csproj", "src/Modules/Events/Evently.Modules.Events.Domain/"]
COPY ["src/Modules/Events/Evently.Modules.Events.Presentation/Evently.Modules.Events.Presentation.csproj", "src/Modules/Events/Evently.Modules.Events.Presentation/"]
COPY ["src/Modules/Users/Evently.Modules.Users.Infrastructure/Evently.Modules.Users.Infrastructure.csproj", "src/Modules/Users/Evently.Modules.Users.Infrastructure/"]
COPY ["src/Modules/Users/Evently.Modules.Users.Application/Evently.Modules.Users.Application.csproj", "src/Modules/Users/Evently.Modules.Users.Application/"]
COPY ["src/Modules/Users/Evently.Modules.Users.Domain/Evently.Modules.Users.Domain.csproj", "src/Modules/Users/Evently.Modules.Users.Domain/"]
COPY ["src/Modules/Users/Evently.Modules.Users.Presentation/Evently.Modules.Users.Presentation.csproj", "src/Modules/Users/Evently.Modules.Users.Presentation/"]
RUN dotnet restore "./src/API/Evently.Api/Evently.Api.csproj"
COPY . .
WORKDIR "/src/src/API/Evently.Api"
RUN dotnet build "./Evently.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Evently.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Evently.Api.dll"]