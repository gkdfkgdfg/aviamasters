using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application.Common.Interfaces;
using WarehouseManagement.Application.Inventory.Dtos;
using WarehouseManagement.Domain.Entities;
using WarehouseManagement.Domain.Enums;

namespace WarehouseManagement.Application.Inventory.Commands;

public record RegisterInboundOrderCommand(
    Guid SupplierId,
    Guid WarehouseId,
    DateTime ExpectedArrivalDateUtc,
    string? Notes,
    IReadOnlyCollection<RegisterInboundOrderLine> Lines
) : IRequest<InboundOrderDto>;

public record RegisterInboundOrderLine(Guid ProductId, int OrderedQuantity, decimal UnitPrice);

public class RegisterInboundOrderCommandValidator : AbstractValidator<RegisterInboundOrderCommand>
{
    public RegisterInboundOrderCommandValidator()
    {
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ExpectedArrivalDateUtc).GreaterThan(DateTime.UtcNow.AddDays(-1));
        RuleFor(x => x.Lines).NotEmpty();
        RuleForEach(x => x.Lines).SetValidator(new RegisterInboundOrderLineValidator());
    }
}

public class RegisterInboundOrderLineValidator : AbstractValidator<RegisterInboundOrderLine>
{
    public RegisterInboundOrderLineValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.OrderedQuantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
    }
}

public class RegisterInboundOrderCommandHandler : IRequestHandler<RegisterInboundOrderCommand, InboundOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RegisterInboundOrderCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InboundOrderDto> Handle(RegisterInboundOrderCommand request, CancellationToken cancellationToken)
    {
        var supplierExists = await _context.Suppliers.AnyAsync(s => s.Id == request.SupplierId, cancellationToken);
        if (!supplierExists)
        {
            throw new KeyNotFoundException($"Supplier {request.SupplierId} not found");
        }

        var warehouseExists = await _context.Warehouses.AnyAsync(w => w.Id == request.WarehouseId, cancellationToken);
        if (!warehouseExists)
        {
            throw new KeyNotFoundException($"Warehouse {request.WarehouseId} not found");
        }

        var products = await _context.Products
            .Where(p => request.Lines.Select(l => l.ProductId).Contains(p.Id))
            .ToListAsync(cancellationToken);

        if (products.Count != request.Lines.Count)
        {
            throw new InvalidOperationException("One or more products could not be found.");
        }

        var order = new InboundOrder
        {
            SupplierId = request.SupplierId,
            WarehouseId = request.WarehouseId,
            ExpectedArrivalDateUtc = request.ExpectedArrivalDateUtc,
            Status = InboundOrderStatus.Submitted,
            Notes = request.Notes,
            OrderNumber = GenerateOrderNumber()
        };

        foreach (var line in request.Lines)
        {
            order.Lines.Add(new InboundOrderLine
            {
                ProductId = line.ProductId,
                OrderedQuantity = line.OrderedQuantity,
                UnitPrice = line.UnitPrice,
                ReceivedQuantity = 0
            });
        }

        _context.InboundOrders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(order).Reference(o => o.Supplier).LoadAsync(cancellationToken);
        await _context.Entry(order).Reference(o => o.Warehouse).LoadAsync(cancellationToken);
        await _context.Entry(order).Collection(o => o.Lines).Query().Include(l => l.Product).LoadAsync(cancellationToken);

        return _mapper.Map<InboundOrderDto>(order);
    }

    private static string GenerateOrderNumber()
    {
        return $"PO-{DateTime.UtcNow:yyyyMMddHHmmss}";
    }
}
