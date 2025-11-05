using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Data.Configurations;

public class OutboundOrderConfiguration : IEntityTypeConfiguration<OutboundOrder>
{
    public void Configure(EntityTypeBuilder<OutboundOrder> builder)
    {
        builder.ToTable("outbound_orders");
        builder.HasIndex(o => o.DocumentNumber).IsUnique();
        builder.Property(o => o.DocumentNumber).IsRequired().HasMaxLength(50);
        builder.Property(o => o.DestinationDepartment).HasMaxLength(200);
        builder.Property(o => o.RequestedBy).HasMaxLength(200);
        builder.Property(o => o.Notes).HasMaxLength(1000);

        builder.HasMany(o => o.Lines)
            .WithOne(l => l.OutboundOrder!)
            .HasForeignKey(l => l.OutboundOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
