using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Data.Configurations;

public class OutboundOrderLineConfiguration : IEntityTypeConfiguration<OutboundOrderLine>
{
    public void Configure(EntityTypeBuilder<OutboundOrderLine> builder)
    {
        builder.ToTable("outbound_order_lines");
        builder.Property(l => l.RequestedQuantity).IsRequired();
        builder.Property(l => l.ReleasedQuantity).IsRequired();
    }
}
