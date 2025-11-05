using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application.Common.Interfaces;
using WarehouseManagement.Application.Products.Dtos;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Sku,
    Guid CategoryId,
    string? Description,
    decimal UnitPrice,
    string UnitOfMeasure,
    int MinimumStockLevel
) : IRequest<ProductDto>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Sku).NotEmpty().MaximumLength(100);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.UnitOfMeasure).NotEmpty().MaximumLength(50);
        RuleFor(x => x.MinimumStockLevel).GreaterThanOrEqualTo(0);
    }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var existing = await _context.Products
            .AnyAsync(p => p.Sku == request.Sku, cancellationToken);

        if (existing)
        {
            throw new InvalidOperationException($"Product with SKU {request.Sku} already exists.");
        }

        var product = new Product
        {
            Name = request.Name,
            Sku = request.Sku,
            CategoryId = request.CategoryId,
            Description = request.Description,
            UnitPrice = request.UnitPrice,
            UnitOfMeasure = request.UnitOfMeasure,
            MinimumStockLevel = request.MinimumStockLevel,
            IsActive = true
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(product).Reference(p => p.Category).LoadAsync(cancellationToken);

        return _mapper.Map<ProductDto>(product);
    }
}
