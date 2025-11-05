using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasIndex(p => p.Sku).IsUnique();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Sku).IsRequired().HasMaxLength(100);
        builder.Property(p => p.UnitOfMeasure).IsRequired().HasMaxLength(50);
        builder.Property(p => p.UnitPrice).HasPrecision(18, 2);
        builder.Property(p => p.AveragePurchasePrice).HasPrecision(18, 2);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
