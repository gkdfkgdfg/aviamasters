using WarehouseManagement.Domain.Enums;

namespace WarehouseManagement.Domain.Entities;

public class InboundOrder : BaseEntity
{
    public required string OrderNumber { get; set; }
    public DateTime ExpectedArrivalDateUtc { get; set; }
    public InboundOrderStatus Status { get; set; } = InboundOrderStatus.Draft;
    public string? Notes { get; set; }

    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }

    public ICollection<InboundOrderLine> Lines { get; set; } = new List<InboundOrderLine>();
    public ICollection<InboundShipment> Shipments { get; set; } = new List<InboundShipment>();
}
