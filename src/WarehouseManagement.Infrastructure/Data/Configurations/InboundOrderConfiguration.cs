using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Data.Configurations;

public class InboundOrderConfiguration : IEntityTypeConfiguration<InboundOrder>
{
    public void Configure(EntityTypeBuilder<InboundOrder> builder)
    {
        builder.ToTable("inbound_orders");
        builder.HasIndex(o => o.OrderNumber).IsUnique();
        builder.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
        builder.Property(o => o.Notes).HasMaxLength(1000);

        builder.HasOne(o => o.Warehouse)
            .WithMany(w => w.InboundOrders)
            .HasForeignKey(o => o.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(o => o.Lines)
            .WithOne(l => l.InboundOrder!)
            .HasForeignKey(l => l.InboundOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
