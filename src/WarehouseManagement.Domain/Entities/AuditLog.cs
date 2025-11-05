namespace WarehouseManagement.Domain.Entities;

public class AuditLog : BaseEntity
{
    public required string EntityName { get; set; }
    public Guid EntityId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Changes { get; set; }
    public string? IpAddress { get; set; }
}
