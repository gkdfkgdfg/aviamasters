using MediatR;
using WarehouseManagement.Application.Inventory.Commands;
using WarehouseManagement.Application.Inventory.Dtos;
using WarehouseManagement.Application.Inventory.Queries;

namespace WarehouseManagement.Api.Endpoints;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory");

        group.MapGet("/inbound-orders", async (Guid? supplierId, InboundOrdersFilterStatus status, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInboundOrdersQuery(supplierId, status), ct);
            return Results.Ok(result);
        })
        .WithName("GetInboundOrders")
        .Produces<IReadOnlyCollection<InboundOrderDto>>(StatusCodes.Status200OK)
        .WithOpenApi();

        group.MapPost("/inbound-orders", async (RegisterInboundOrderRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var lines = request.Lines.Select(l => new RegisterInboundOrderLine(l.ProductId, l.OrderedQuantity, l.UnitPrice)).ToList();
            var command = new RegisterInboundOrderCommand(request.SupplierId, request.WarehouseId, request.ExpectedArrivalDateUtc, request.Notes, lines);
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/inventory/inbound-orders/{result.Id}", result);
        })
        .RequireAuthorization()
        .WithName("RegisterInboundOrder")
        .Produces<InboundOrderDto>(StatusCodes.Status201Created)
        .WithOpenApi();

        group.MapPost("/inbound-orders/{id:guid}/receive", async (Guid id, ReceiveInboundShipmentRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var lines = request.Lines.Select(l => new ReceiveInboundShipmentLine(l.ProductId, l.QuantityReceived, l.UnitPrice)).ToList();
            var command = new ReceiveInboundShipmentCommand(id, request.ReceivedAtUtc, request.DocumentNumber, request.Notes, lines);
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("ReceiveInboundShipment")
        .Produces<InboundOrderDto>(StatusCodes.Status200OK)
        .WithOpenApi();
    }
}

public record RegisterInboundOrderRequest(
    Guid SupplierId,
    Guid WarehouseId,
    DateTime ExpectedArrivalDateUtc,
    string? Notes,
    IReadOnlyCollection<RegisterInboundOrderLineRequest> Lines
);

public record RegisterInboundOrderLineRequest(Guid ProductId, int OrderedQuantity, decimal UnitPrice);

public record ReceiveInboundShipmentRequest(
    DateTime ReceivedAtUtc,
    string? DocumentNumber,
    string? Notes,
    IReadOnlyCollection<ReceiveInboundShipmentLineRequest> Lines
);

public record ReceiveInboundShipmentLineRequest(Guid ProductId, int QuantityReceived, decimal UnitPrice);
