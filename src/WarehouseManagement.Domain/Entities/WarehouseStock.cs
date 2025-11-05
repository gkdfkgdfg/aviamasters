namespace WarehouseManagement.Domain.Entities;

public class WarehouseStock : BaseEntity
{
    public Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public int QuantityOnHand { get; set; }
    public int QuantityReserved { get; set; }
    public DateTime? LastInboundDateUtc { get; set; }
    public DateTime? LastOutboundDateUtc { get; set; }
}
