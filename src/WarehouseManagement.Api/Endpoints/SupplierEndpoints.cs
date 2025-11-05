using MediatR;
using WarehouseManagement.Application.Suppliers.Commands;
using WarehouseManagement.Application.Suppliers.Dtos;
using WarehouseManagement.Application.Suppliers.Queries;

namespace WarehouseManagement.Api.Endpoints;

public static class SupplierEndpoints
{
    public static void MapSupplierEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/suppliers");

        group.MapGet("", async (string? search, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSuppliersQuery(search), ct);
            return Results.Ok(result);
        })
        .WithName("GetSuppliers")
        .Produces<IReadOnlyCollection<SupplierDto>>(StatusCodes.Status200OK)
        .WithOpenApi();

        group.MapPost("", async (CreateSupplierRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateSupplierCommand(
                request.Name,
                request.TaxId,
                request.Email,
                request.Phone,
                request.ContactPerson,
                request.PaymentTerms,
                request.IsPreferred);

            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/suppliers/{result.Id}", result);
        })
        .RequireAuthorization()
        .WithName("CreateSupplier")
        .Produces<SupplierDto>(StatusCodes.Status201Created)
        .WithOpenApi();
    }
}

public record CreateSupplierRequest(
    string Name,
    string? TaxId,
    string? Email,
    string? Phone,
    string? ContactPerson,
    string? PaymentTerms,
    bool IsPreferred
);
