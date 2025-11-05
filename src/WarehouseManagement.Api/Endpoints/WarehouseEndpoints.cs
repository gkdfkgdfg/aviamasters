using MediatR;
using WarehouseManagement.Application.Warehouses.Commands;
using WarehouseManagement.Application.Warehouses.Dtos;
using WarehouseManagement.Application.Warehouses.Queries;

namespace WarehouseManagement.Api.Endpoints;

public static class WarehouseEndpoints
{
    public static void MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/warehouses");

        group.MapGet("", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetWarehousesQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetWarehouses")
        .Produces<IReadOnlyCollection<WarehouseDto>>(StatusCodes.Status200OK)
        .WithOpenApi();

        group.MapPost("", async (CreateWarehouseRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateWarehouseCommand(request.Name, request.Address, request.ResponsiblePerson);
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/warehouses/{result.Id}", result);
        })
        .RequireAuthorization()
        .WithName("CreateWarehouse")
        .Produces<WarehouseDto>(StatusCodes.Status201Created)
        .WithOpenApi();
    }
}

public record CreateWarehouseRequest(string Name, string? Address, string? ResponsiblePerson);
