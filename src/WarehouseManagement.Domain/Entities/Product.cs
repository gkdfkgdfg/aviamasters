namespace WarehouseManagement.Domain.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public string? Barcode { get; set; }
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? AveragePurchasePrice { get; set; }
    public string UnitOfMeasure { get; set; } = "pcs";
    public int MinimumStockLevel { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
    public ICollection<InboundOrderLine> InboundOrderLines { get; set; } = new List<InboundOrderLine>();
    public ICollection<OutboundOrderLine> OutboundOrderLines { get; set; } = new List<OutboundOrderLine>();
}
