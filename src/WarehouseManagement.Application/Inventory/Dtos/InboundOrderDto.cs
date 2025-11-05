using WarehouseManagement.Domain.Enums;

namespace WarehouseManagement.Application.Inventory.Dtos;

public record InboundOrderLineDto(
    Guid ProductId,
    string ProductName,
    int OrderedQuantity,
    int ReceivedQuantity,
    decimal UnitPrice
);

public record InboundOrderDto(
    Guid Id,
    string OrderNumber,
    Guid SupplierId,
    string SupplierName,
    Guid WarehouseId,
    string WarehouseName,
    DateTime ExpectedArrivalDateUtc,
    InboundOrderStatus Status,
    IReadOnlyCollection<InboundOrderLineDto> Lines
);
