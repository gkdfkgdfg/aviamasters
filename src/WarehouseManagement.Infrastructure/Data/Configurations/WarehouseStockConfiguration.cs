using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Data.Configurations;

public class WarehouseStockConfiguration : IEntityTypeConfiguration<WarehouseStock>
{
    public void Configure(EntityTypeBuilder<WarehouseStock> builder)
    {
        builder.ToTable("warehouse_stocks");
        builder.HasIndex(ws => new { ws.WarehouseId, ws.ProductId }).IsUnique();
        builder.Property(ws => ws.QuantityOnHand).IsRequired();
        builder.Property(ws => ws.QuantityReserved).IsRequired();
    }
}
