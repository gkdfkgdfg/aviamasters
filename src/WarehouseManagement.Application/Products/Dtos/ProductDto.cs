namespace WarehouseManagement.Application.Products.Dtos;

public record ProductDto(
    Guid Id,
    string Name,
    string Sku,
    string UnitOfMeasure,
    int MinimumStockLevel,
    decimal UnitPrice,
    Guid CategoryId,
    string CategoryName,
    bool IsActive
);
