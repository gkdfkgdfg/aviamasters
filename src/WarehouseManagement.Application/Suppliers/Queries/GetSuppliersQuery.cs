using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application.Common.Interfaces;
using WarehouseManagement.Application.Suppliers.Dtos;

namespace WarehouseManagement.Application.Suppliers.Queries;

public record GetSuppliersQuery(string? Search) : IRequest<IReadOnlyCollection<SupplierDto>>;

public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, IReadOnlyCollection<SupplierDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSuppliersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<SupplierDto>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Suppliers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLowerInvariant();
            query = query.Where(s => s.Name.ToLower().Contains(term));
        }

        return await query
            .OrderBy(s => s.Name)
            .ProjectTo<SupplierDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
