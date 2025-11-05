using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Data.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("warehouses");
        builder.Property(w => w.Name).IsRequired().HasMaxLength(200);
        builder.Property(w => w.Address).HasMaxLength(500);
        builder.Property(w => w.ResponsiblePerson).HasMaxLength(200);
    }
}
