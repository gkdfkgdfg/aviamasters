using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Paper Products" },
                new Category { Name = "Writing Instruments" },
                new Category { Name = "Office Equipment" }
            );
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!context.Warehouses.Any())
        {
            context.Warehouses.AddRange(
                new Warehouse { Name = "Main Warehouse", Address = "Moscow, Central street 1" },
                new Warehouse { Name = "Reserve Warehouse", Address = "Moscow, Backup street 5" }
            );
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!context.Suppliers.Any())
        {
            context.Suppliers.AddRange(
                new Supplier { Name = "Paper Plus LLC", Email = "info@paperplus.example", Phone = "+7 (495) 000-00-01" },
                new Supplier { Name = "Office Market LLC", Email = "sales@officemarket.example", Phone = "+7 (495) 000-00-02" }
            );
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
