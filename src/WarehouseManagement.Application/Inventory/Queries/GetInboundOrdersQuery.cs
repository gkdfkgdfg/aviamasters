using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application.Common.Interfaces;
using WarehouseManagement.Application.Inventory.Dtos;

namespace WarehouseManagement.Application.Inventory.Queries;

public record GetInboundOrdersQuery(Guid? SupplierId, InboundOrdersFilterStatus Status) : IRequest<IReadOnlyCollection<InboundOrderDto>>;

public enum InboundOrdersFilterStatus
{
    Any,
    Submitted,
    PartiallyReceived,
    Received
}

public class GetInboundOrdersQueryHandler : IRequestHandler<GetInboundOrdersQuery, IReadOnlyCollection<InboundOrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInboundOrdersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<InboundOrderDto>> Handle(GetInboundOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.InboundOrders
            .Include(o => o.Warehouse)
            .Include(o => o.Supplier)
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product)
            .AsQueryable();

        if (request.SupplierId.HasValue)
        {
            query = query.Where(o => o.SupplierId == request.SupplierId);
        }

        query = request.Status switch
        {
            InboundOrdersFilterStatus.Submitted => query.Where(o => o.Status == Domain.Enums.InboundOrderStatus.Submitted),
            InboundOrdersFilterStatus.PartiallyReceived => query.Where(o => o.Status == Domain.Enums.InboundOrderStatus.PartiallyReceived),
            InboundOrdersFilterStatus.Received => query.Where(o => o.Status == Domain.Enums.InboundOrderStatus.Received),
            _ => query
        };

        return await query
            .OrderByDescending(o => o.CreatedAtUtc)
            .ProjectTo<InboundOrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
