using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Data.Configurations;

public class InventorySessionConfiguration : IEntityTypeConfiguration<InventorySession>
{
    public void Configure(EntityTypeBuilder<InventorySession> builder)
    {
        builder.ToTable("inventory_sessions");
        builder.Property(s => s.Code).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Notes).HasMaxLength(1000);

        builder.HasMany(s => s.Counts)
            .WithOne(c => c.InventorySession!)
            .HasForeignKey(c => c.InventorySessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Adjustments)
            .WithOne(a => a.InventorySession!)
            .HasForeignKey(a => a.InventorySessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
