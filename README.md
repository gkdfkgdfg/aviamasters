# Warehouse Management Information System

ASP.NET Core 9.0 solution for a small-office stationery warehouse with PostgreSQL storage, modular architecture, MediatR command/query handling, Entity Framework Core, and minimal API endpoints. The project targets Visual Studio 2022 and supports containerised deployment.

## Solution Structure

- `WarehouseManagement.Api` – Minimal API entry point, JWT auth stub, Swagger, health checks.
- `WarehouseManagement.Application` – CQRS handlers, DTOs, validators, application services.
- `WarehouseManagement.Domain` – Entity models and enumerations.
- `WarehouseManagement.Infrastructure` – EF Core context, PostgreSQL configuration, seed data, cross-cutting services.
- `WarehouseManagement.Tests` – xUnit tests for application layer.
- `docs/requirements.md` – gathered business and non-functional requirements.
- `scripts/initial_schema.sql` – baseline SQL script for manual database provisioning.

## Prerequisites

- .NET SDK 9.0 preview.
- PostgreSQL 16 (or compatible).
- Visual Studio 2022 17.10+ or JetBrains Rider 2024.2+.
- Optional: Docker/Docker Compose, Power BI Desktop, Excel, Draw.io / StarUML for diagrams.

## Getting Started

### Option A — Docker (рекомендуется для быстрого запуска)

1. Установите Docker Desktop / Docker Engine.
2. В корне проекта выполните:
   ```bash
   docker compose up --build
   ```
3. API будет доступен на `http://localhost:8080` (Swagger: `http://localhost:8080/swagger`).
4. PostgreSQL доступен на `localhost:5432` с учётными данными `warehouse_user / warehouse_pwd`.

### Option B — Локально с Visual Studio / CLI

1. Установите .NET SDK 9.0 Preview и PostgreSQL 16.
2. Откройте `WarehouseManagement.sln` в Visual Studio 2022 (17.10+) или выполните команды из корня:
   ```bash
   dotnet restore
   dotnet build
   ```
3. Настройте строку подключения и JWT-секрет:
   ```bash
   dotnet restore
   dotnet build
   ```
3. Configure connection string and JWT secret:
   - Copy `src/WarehouseManagement.Api/appsettings.json` to an environment-specific file or override values via environment variables: 
     - `ConnectionStrings__DefaultConnection`
     - `Jwt__Issuer`, `Jwt__Audience`, `Jwt__Secret`
4. Apply EF Core migrations (автоматически выполняются при старте). Для ручного управления миграциями:
   ```bash
   dotnet ef migrations add InitialCreate --project src/WarehouseManagement.Infrastructure --startup-project src/WarehouseManagement.Api
   dotnet ef database update --project src/WarehouseManagement.Infrastructure --startup-project src/WarehouseManagement.Api
   ```
   Alternatively, execute `scripts/initial_schema.sql` against an empty database.
5. Запустите API:
   ```bash
   dotnet run --project src/WarehouseManagement.Api
   ```
6. Откройте `https://localhost:5001/swagger` (или адрес, который показывает Kestrel) для документации.

## Running Tests

```bash
dotnet test WarehouseManagement.sln
```

The test project currently covers the product command handler. Extend with additional unit/integration tests as features expand.

## Feature Highlights

- Products: CRUD with validation, category linking, soft-delete.
- Suppliers: creation and listing with basic validation.
- Warehouses: creation and listing.
- Inventory inbound flow: register purchase orders, receive shipments, auto-update stock levels.
- EF Core with PostgreSQL provider, automatic migrations and sample seed data.
- MediatR pipeline with FluentValidation.
- Serilog console logging, health checks, Swagger/OpenAPI.

## Reporting and Analytics

- **Excel**: use `/api/products`, `/api/inventory/inbound-orders`, etc. to export CSV/JSON for pivot tables.
- **Power BI**: create a DirectQuery connection to PostgreSQL; sample dashboards can highlight stock coverage, inbound/outbound volumes, and supplier KPIs.

## Modelling Assets

- Use `docs/requirements.md` as a base for UML diagrams (use-case, sequence, ER). Suggested tooling: Draw.io or StarUML.
- Entity relationships are codified in `WarehouseManagement.Domain`; reflect them in the ER diagram for documentation.

## Deployment Notes

- Containerisation: create a `Dockerfile` for the API and a `docker-compose.yml` with PostgreSQL service for local stacks.
- CI/CD: configure GitHub Actions or Azure DevOps to restore, build, test, and publish Docker images to a registry.
- Backups: schedule `pg_dump` for nightly backups and verify restore procedures.

## Next Steps

- Implement outbound order workflows and inventory adjustments endpoints.
- Integrate a real identity provider (ASP.NET Identity, external IdP) and replace the placeholder JWT secret handling.
- Add audit logging middleware and history tables.
- Expand automated test coverage (integration tests with Testcontainers for PostgreSQL).
