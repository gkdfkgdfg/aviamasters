using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Data.Configurations;

public class InboundShipmentLineConfiguration : IEntityTypeConfiguration<InboundShipmentLine>
{
    public void Configure(EntityTypeBuilder<InboundShipmentLine> builder)
    {
        builder.ToTable("inbound_shipment_lines");
        builder.Property(l => l.QuantityReceived).IsRequired();
        builder.Property(l => l.UnitPrice).HasPrecision(18, 2);
    }
}
