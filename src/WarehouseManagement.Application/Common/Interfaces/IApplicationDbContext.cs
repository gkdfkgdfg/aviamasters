using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DbSet<Supplier> Suppliers { get; }
    DbSet<Warehouse> Warehouses { get; }
    DbSet<WarehouseStock> WarehouseStocks { get; }
    DbSet<InboundOrder> InboundOrders { get; }
    DbSet<InboundOrderLine> InboundOrderLines { get; }
    DbSet<InboundShipment> InboundShipments { get; }
    DbSet<InboundShipmentLine> InboundShipmentLines { get; }
    DbSet<OutboundOrder> OutboundOrders { get; }
    DbSet<OutboundOrderLine> OutboundOrderLines { get; }
    DbSet<InventorySession> InventorySessions { get; }
    DbSet<InventoryCount> InventoryCounts { get; }
    DbSet<InventoryAdjustment> InventoryAdjustments { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<UserAccount> UserAccounts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
