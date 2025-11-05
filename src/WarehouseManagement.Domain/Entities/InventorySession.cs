using WarehouseManagement.Domain.Enums;

namespace WarehouseManagement.Domain.Entities;

public class InventorySession : BaseEntity
{
    public required string Code { get; set; }
    public InventorySessionStatus Status { get; set; } = InventorySessionStatus.Planned;
    public DateTime ScheduledDateUtc { get; set; }
    public DateTime? CompletedDateUtc { get; set; }
    public string? Notes { get; set; }

    public Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }

    public ICollection<InventoryCount> Counts { get; set; } = new List<InventoryCount>();
    public ICollection<InventoryAdjustment> Adjustments { get; set; } = new List<InventoryAdjustment>();
}
