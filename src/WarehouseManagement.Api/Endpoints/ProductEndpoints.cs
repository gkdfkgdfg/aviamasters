using MediatR;
using WarehouseManagement.Application.Products.Commands;
using WarehouseManagement.Application.Products.Dtos;
using WarehouseManagement.Application.Products.Queries;

namespace WarehouseManagement.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products");

        group.MapGet("", async (string? search, Guid? categoryId, bool includeInactive, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProductsQuery(search, categoryId, includeInactive), ct);
            return Results.Ok(result);
        })
        .WithName("GetProducts")
        .Produces<IReadOnlyCollection<ProductDto>>(StatusCodes.Status200OK)
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProductByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetProductById")
        .Produces<ProductDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi();

        group.MapPost("", async (CreateProductRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateProductCommand(
                request.Name,
                request.Sku,
                request.CategoryId,
                request.Description,
                request.UnitPrice,
                request.UnitOfMeasure,
                request.MinimumStockLevel);

            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/products/{result.Id}", result);
        })
        .RequireAuthorization()
        .WithName("CreateProduct")
        .Produces<ProductDto>(StatusCodes.Status201Created)
        .WithOpenApi();

        group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var command = new UpdateProductCommand(
                id,
                request.Name,
                request.Description,
                request.UnitPrice,
                request.UnitOfMeasure,
                request.MinimumStockLevel,
                request.IsActive);

            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("UpdateProduct")
        .Produces<ProductDto>(StatusCodes.Status200OK)
        .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteProductCommand(id), ct);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteProduct")
        .Produces(StatusCodes.Status204NoContent)
        .WithOpenApi();
    }
}

public record CreateProductRequest(
    string Name,
    string Sku,
    Guid CategoryId,
    string? Description,
    decimal UnitPrice,
    string UnitOfMeasure,
    int MinimumStockLevel
);

public record UpdateProductRequest(
    string Name,
    string? Description,
    decimal UnitPrice,
    string UnitOfMeasure,
    int MinimumStockLevel,
    bool IsActive
);
