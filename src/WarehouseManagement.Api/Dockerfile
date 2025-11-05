# syntax=docker/dockerfile:1.7

FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8443

FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

COPY ["WarehouseManagement.sln", "./"]
COPY ["src/WarehouseManagement.Api/WarehouseManagement.Api.csproj", "src/WarehouseManagement.Api/"]
COPY ["src/WarehouseManagement.Application/WarehouseManagement.Application.csproj", "src/WarehouseManagement.Application/"]
COPY ["src/WarehouseManagement.Infrastructure/WarehouseManagement.Infrastructure.csproj", "src/WarehouseManagement.Infrastructure/"]
COPY ["src/WarehouseManagement.Domain/WarehouseManagement.Domain.csproj", "src/WarehouseManagement.Domain/"]
COPY ["tests/WarehouseManagement.Tests/WarehouseManagement.Tests.csproj", "tests/WarehouseManagement.Tests/"]

RUN dotnet restore "WarehouseManagement.sln"

COPY . .

RUN dotnet publish "src/WarehouseManagement.Api/WarehouseManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WarehouseManagement.Api.dll"]
