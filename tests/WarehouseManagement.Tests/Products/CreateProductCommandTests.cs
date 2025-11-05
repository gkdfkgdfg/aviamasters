using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application.Common.Interfaces;
using WarehouseManagement.Application.Products.Commands;
using WarehouseManagement.Application.Products.Dtos;
using WarehouseManagement.Application.Products;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Tests.Products;

public class CreateProductCommandTests
{
    private readonly IMapper _mapper;

    public CreateProductCommandTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ProductMappingProfile>());
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_Create_Product_When_Sku_Is_Unique()
    {
        var options = new DbContextOptionsBuilder<InMemoryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new InMemoryContext(options);
        var handler = new CreateProductCommandHandler(context, _mapper);

        context.Categories.Add(new Category { Id = Guid.NewGuid(), Name = "Paper" });
        await context.SaveChangesAsync();

        var command = new CreateProductCommand(
            "Printer Paper A4",
            "PP-A4-001",
            context.Categories.First().Id,
            "A4 size printer paper",
            250,
            "pack",
            10);

        ProductDto result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Printer Paper A4");
        context.Products.Should().HaveCount(1);
    }

    private sealed class InMemoryContext : DbContext, IApplicationDbContext
    {
        public InMemoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<WarehouseStock> WarehouseStocks => Set<WarehouseStock>();
        public DbSet<InboundOrder> InboundOrders => Set<InboundOrder>();
        public DbSet<InboundOrderLine> InboundOrderLines => Set<InboundOrderLine>();
        public DbSet<InboundShipment> InboundShipments => Set<InboundShipment>();
        public DbSet<InboundShipmentLine> InboundShipmentLines => Set<InboundShipmentLine>();
        public DbSet<OutboundOrder> OutboundOrders => Set<OutboundOrder>();
        public DbSet<OutboundOrderLine> OutboundOrderLines => Set<OutboundOrderLine>();
        public DbSet<InventorySession> InventorySessions => Set<InventorySession>();
        public DbSet<InventoryCount> InventoryCounts => Set<InventoryCount>();
        public DbSet<InventoryAdjustment> InventoryAdjustments => Set<InventoryAdjustment>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    }
}
