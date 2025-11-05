using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Data.Configurations;

public class InboundShipmentConfiguration : IEntityTypeConfiguration<InboundShipment>
{
    public void Configure(EntityTypeBuilder<InboundShipment> builder)
    {
        builder.ToTable("inbound_shipments");
        builder.Property(s => s.DocumentNumber).HasMaxLength(100);
        builder.Property(s => s.Notes).HasMaxLength(1000);

        builder.HasMany(s => s.Lines)
            .WithOne(l => l.InboundShipment!)
            .HasForeignKey(l => l.InboundShipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
