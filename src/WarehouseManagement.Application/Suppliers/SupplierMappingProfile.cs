using AutoMapper;
using WarehouseManagement.Application.Suppliers.Dtos;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Suppliers;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<Supplier, SupplierDto>();
    }
}
