using AutoMapper;
using FluentValidation;
using MediatR;
using WarehouseManagement.Application.Common.Interfaces;
using WarehouseManagement.Application.Warehouses.Dtos;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Warehouses.Commands;

public record CreateWarehouseCommand(
    string Name,
    string? Address,
    string? ResponsiblePerson
) : IRequest<WarehouseDto>;

public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, WarehouseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateWarehouseCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WarehouseDto> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Warehouse
        {
            Name = request.Name,
            Address = request.Address,
            ResponsiblePerson = request.ResponsiblePerson
        };

        _context.Warehouses.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WarehouseDto>(entity);
    }
}
