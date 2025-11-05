using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Data.Configurations;

public class InboundOrderLineConfiguration : IEntityTypeConfiguration<InboundOrderLine>
{
    public void Configure(EntityTypeBuilder<InboundOrderLine> builder)
    {
        builder.ToTable("inbound_order_lines");
        builder.Property(l => l.OrderedQuantity).IsRequired();
        builder.Property(l => l.ReceivedQuantity).IsRequired();
        builder.Property(l => l.UnitPrice).HasPrecision(18, 2);
    }
}
