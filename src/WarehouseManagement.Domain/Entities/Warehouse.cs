namespace WarehouseManagement.Domain.Entities;

public class Warehouse : BaseEntity
{
    public required string Name { get; set; }
    public string? Address { get; set; }
    public string? ResponsiblePerson { get; set; }

    public ICollection<WarehouseStock> Stocks { get; set; } = new List<WarehouseStock>();
    public ICollection<InboundOrder> InboundOrders { get; set; } = new List<InboundOrder>();
    public ICollection<OutboundOrder> OutboundOrders { get; set; } = new List<OutboundOrder>();
    public ICollection<InventorySession> InventorySessions { get; set; } = new List<InventorySession>();
}
