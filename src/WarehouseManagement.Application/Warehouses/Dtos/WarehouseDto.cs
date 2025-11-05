namespace WarehouseManagement.Application.Warehouses.Dtos;

public record WarehouseDto(
    Guid Id,
    string Name,
    string? Address,
    string? ResponsiblePerson
);
