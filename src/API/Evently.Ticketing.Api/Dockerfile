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
COPY ["src/API/Evently.Ticketing.Api/Evently.Ticketing.Api.csproj", "src/API/Evently.Ticketing.Api/"]
COPY ["src/Modules/Ticketing/Evently.Modules.Ticketing.Infrastructure/Evently.Modules.Ticketing.Infrastructure.csproj", "src/Modules/Ticketing/Evently.Modules.Ticketing.Infrastructure/"]
COPY ["src/Shared/Evently.Shared.Infrastructure/Evently.Shared.Infrastructure.csproj", "src/Shared/Evently.Shared.Infrastructure/"]
COPY ["src/Shared/Evently.Shared.Application/Evently.Shared.Application.csproj", "src/Shared/Evently.Shared.Application/"]
COPY ["src/Shared/Evently.Shared.Domain/Evently.Shared.Domain.csproj", "src/Shared/Evently.Shared.Domain/"]
COPY ["src/Modules/Ticketing/Evently.Modules.Ticketing.Application/Evently.Modules.Ticketing.Application.csproj", "src/Modules/Ticketing/Evently.Modules.Ticketing.Application/"]
COPY ["src/Modules/Ticketing/Evently.Modules.Ticketing.Domain/Evently.Modules.Ticketing.Domain.csproj", "src/Modules/Ticketing/Evently.Modules.Ticketing.Domain/"]
COPY ["src/Modules/Ticketing/Evently.Modules.Ticketing.IntegrationEvents/Evently.Modules.Ticketing.IntegrationEvents.csproj", "src/Modules/Ticketing/Evently.Modules.Ticketing.IntegrationEvents/"]
COPY ["src/Modules/Ticketing/Evently.Modules.Ticketing.Presentation/Evently.Modules.Ticketing.Presentation.csproj", "src/Modules/Ticketing/Evently.Modules.Ticketing.Presentation/"]
COPY ["src/Shared/Evently.Shared.Presentation/Evently.Shared.Presentation.csproj", "src/Shared/Evently.Shared.Presentation/"]
COPY ["src/Modules/Events/Evently.Modules.Events.IntegrationEvents/Evently.Modules.Events.IntegrationEvents.csproj", "src/Modules/Events/Evently.Modules.Events.IntegrationEvents/"]
COPY ["src/Modules/Users/Evently.Modules.Users.IntegrationEvents/Evently.Modules.Users.IntegrationEvents.csproj", "src/Modules/Users/Evently.Modules.Users.IntegrationEvents/"]
RUN dotnet restore "./src/API/Evently.Ticketing.Api/Evently.Ticketing.Api.csproj"
COPY . .
WORKDIR "/src/src/API/Evently.Ticketing.Api"
RUN dotnet build "./Evently.Ticketing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Evently.Ticketing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Evently.Ticketing.Api.dll"]