namespace WarehouseManagement.Domain.Entities;

public class InboundShipmentLine : BaseEntity
{
    public Guid InboundShipmentId { get; set; }
    public InboundShipment? InboundShipment { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public int QuantityReceived { get; set; }
    public decimal UnitPrice { get; set; }
}
