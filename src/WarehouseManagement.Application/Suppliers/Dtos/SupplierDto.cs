namespace WarehouseManagement.Application.Suppliers.Dtos;

public record SupplierDto(
    Guid Id,
    string Name,
    string? TaxId,
    string? Email,
    string? Phone,
    string? ContactPerson,
    string? PaymentTerms,
    bool IsPreferred
);
