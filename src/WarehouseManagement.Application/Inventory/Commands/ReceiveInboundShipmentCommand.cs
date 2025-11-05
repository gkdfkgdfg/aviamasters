using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application.Common.Interfaces;
using WarehouseManagement.Application.Inventory.Dtos;
using WarehouseManagement.Domain.Entities;
using WarehouseManagement.Domain.Enums;

namespace WarehouseManagement.Application.Inventory.Commands;

public record ReceiveInboundShipmentCommand(
    Guid InboundOrderId,
    DateTime ReceivedAtUtc,
    string? DocumentNumber,
    string? Notes,
    IReadOnlyCollection<ReceiveInboundShipmentLine> Lines
) : IRequest<InboundOrderDto>;

public record ReceiveInboundShipmentLine(Guid ProductId, int QuantityReceived, decimal UnitPrice);

public class ReceiveInboundShipmentCommandValidator : AbstractValidator<ReceiveInboundShipmentCommand>
{
    public ReceiveInboundShipmentCommandValidator()
    {
        RuleFor(x => x.InboundOrderId).NotEmpty();
        RuleFor(x => x.ReceivedAtUtc).LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5));
        RuleFor(x => x.Lines).NotEmpty();
        RuleForEach(x => x.Lines).SetValidator(new ReceiveInboundShipmentLineValidator());
    }
}

public class ReceiveInboundShipmentLineValidator : AbstractValidator<ReceiveInboundShipmentLine>
{
    public ReceiveInboundShipmentLineValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.QuantityReceived).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
    }
}

public class ReceiveInboundShipmentCommandHandler : IRequestHandler<ReceiveInboundShipmentCommand, InboundOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReceiveInboundShipmentCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<InboundOrderDto> Handle(ReceiveInboundShipmentCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.InboundOrders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product)
            .Include(o => o.Supplier)
            .FirstOrDefaultAsync(o => o.Id == request.InboundOrderId, cancellationToken);

        if (order is null)
        {
            throw new KeyNotFoundException($"Inbound order {request.InboundOrderId} not found");
        }

        if (order.Status == InboundOrderStatus.Received)
        {
            throw new InvalidOperationException("Inbound order already fully received.");
        }

        var shipment = new InboundShipment
        {
            InboundOrderId = order.Id,
            ReceivedAtUtc = request.ReceivedAtUtc,
            DocumentNumber = request.DocumentNumber,
            Notes = request.Notes
        };

        foreach (var line in request.Lines)
        {
            var orderLine = order.Lines.FirstOrDefault(l => l.ProductId == line.ProductId);
            if (orderLine is null)
            {
                throw new InvalidOperationException($"Product {line.ProductId} is not part of the order.");
            }

            orderLine.ReceivedQuantity += line.QuantityReceived;
            shipment.Lines.Add(new InboundShipmentLine
            {
                ProductId = line.ProductId,
                QuantityReceived = line.QuantityReceived,
                UnitPrice = line.UnitPrice
            });

            await AdjustWarehouseStock(order.WarehouseId, line.ProductId, line.QuantityReceived, cancellationToken);
        }

        var allReceived = order.Lines.All(l => l.ReceivedQuantity >= l.OrderedQuantity);
        order.Status = allReceived ? InboundOrderStatus.Received : InboundOrderStatus.PartiallyReceived;
        order.UpdatedAtUtc = _dateTimeProvider.UtcNow;

        _context.InboundShipments.Add(shipment);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<InboundOrderDto>(order);
    }

    private async Task AdjustWarehouseStock(Guid warehouseId, Guid productId, int quantity, CancellationToken cancellationToken)
    {
        var stock = await _context.WarehouseStocks
            .FirstOrDefaultAsync(ws => ws.ProductId == productId && ws.WarehouseId == warehouseId, cancellationToken);

        if (stock is null)
        {
            stock = new WarehouseStock
            {
                WarehouseId = warehouseId,
                ProductId = productId,
                QuantityOnHand = quantity,
                QuantityReserved = 0,
                LastInboundDateUtc = _dateTimeProvider.UtcNow
            };
            _context.WarehouseStocks.Add(stock);
        }
        else
        {
            stock.QuantityOnHand += quantity;
            stock.LastInboundDateUtc = _dateTimeProvider.UtcNow;
        }
    }
}
