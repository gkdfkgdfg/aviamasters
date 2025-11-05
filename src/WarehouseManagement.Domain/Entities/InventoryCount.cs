namespace WarehouseManagement.Domain.Entities;

public class InventoryCount : BaseEntity
{
    public Guid InventorySessionId { get; set; }
    public InventorySession? InventorySession { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public int CountedQuantity { get; set; }
    public int SystemQuantity { get; set; }
}
