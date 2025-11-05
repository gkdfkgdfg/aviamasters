namespace WarehouseManagement.Domain.Entities;

public class InboundShipment : BaseEntity
{
    public Guid InboundOrderId { get; set; }
    public InboundOrder? InboundOrder { get; set; }

    public DateTime ReceivedAtUtc { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Notes { get; set; }

    public ICollection<InboundShipmentLine> Lines { get; set; } = new List<InboundShipmentLine>();
}
