namespace WarehouseManagement.Domain.Entities;

public class Supplier : BaseEntity
{
    public required string Name { get; set; }
    public string? TaxId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? ContactPerson { get; set; }
    public string? PaymentTerms { get; set; }
    public bool IsPreferred { get; set; }

    public ICollection<InboundOrder> InboundOrders { get; set; } = new List<InboundOrder>();
}
