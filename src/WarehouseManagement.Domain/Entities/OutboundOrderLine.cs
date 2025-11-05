namespace WarehouseManagement.Domain.Entities;

public class OutboundOrderLine : BaseEntity
{
    public Guid OutboundOrderId { get; set; }
    public OutboundOrder? OutboundOrder { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public int RequestedQuantity { get; set; }
    public int ReleasedQuantity { get; set; }
}
