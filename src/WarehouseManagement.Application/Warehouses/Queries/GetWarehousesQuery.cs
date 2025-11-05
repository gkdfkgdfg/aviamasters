using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application.Common.Interfaces;
using WarehouseManagement.Application.Warehouses.Dtos;

namespace WarehouseManagement.Application.Warehouses.Queries;

public record GetWarehousesQuery() : IRequest<IReadOnlyCollection<WarehouseDto>>;

public class GetWarehousesQueryHandler : IRequestHandler<GetWarehousesQuery, IReadOnlyCollection<WarehouseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWarehousesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<WarehouseDto>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Warehouses
            .OrderBy(w => w.Name)
            .ProjectTo<WarehouseDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
