namespace WarehouseManagement.Domain.Entities;

public class InventoryAdjustment : BaseEntity
{
    public Guid InventorySessionId { get; set; }
    public InventorySession? InventorySession { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public int AdjustmentQuantity { get; set; }
    public string Reason { get; set; } = string.Empty;
}
