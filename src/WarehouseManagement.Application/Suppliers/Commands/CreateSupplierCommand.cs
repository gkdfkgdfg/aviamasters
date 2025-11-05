using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application.Common.Interfaces;
using WarehouseManagement.Application.Suppliers.Dtos;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Suppliers.Commands;

public record CreateSupplierCommand(
    string Name,
    string? TaxId,
    string? Email,
    string? Phone,
    string? ContactPerson,
    string? PaymentTerms,
    bool IsPreferred
) : IRequest<SupplierDto>;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Phone).MaximumLength(50);
    }
}

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateSupplierCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SupplierDto> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Suppliers
            .AnyAsync(s => s.Name == request.Name, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException($"Supplier with name {request.Name} already exists.");
        }

        var supplier = new Supplier
        {
            Name = request.Name,
            TaxId = request.TaxId,
            Email = request.Email,
            Phone = request.Phone,
            ContactPerson = request.ContactPerson,
            PaymentTerms = request.PaymentTerms,
            IsPreferred = request.IsPreferred
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SupplierDto>(supplier);
    }
}
