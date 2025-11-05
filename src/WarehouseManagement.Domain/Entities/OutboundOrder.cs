using WarehouseManagement.Domain.Enums;

namespace WarehouseManagement.Domain.Entities;

public class OutboundOrder : BaseEntity
{
    public required string DocumentNumber { get; set; }
    public OutboundOrderStatus Status { get; set; } = OutboundOrderStatus.Draft;
    public DateTime RequestedDateUtc { get; set; }
    public DateTime? CompletedDateUtc { get; set; }
    public string? DestinationDepartment { get; set; }
    public string? RequestedBy { get; set; }
    public string? Notes { get; set; }

    public Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }

    public ICollection<OutboundOrderLine> Lines { get; set; } = new List<OutboundOrderLine>();
}
