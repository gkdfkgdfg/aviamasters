namespace WarehouseManagement.Domain.Entities;

public class InboundOrderLine : BaseEntity
{
    public Guid InboundOrderId { get; set; }
    public InboundOrder? InboundOrder { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public int OrderedQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int ReceivedQuantity { get; set; }
}
