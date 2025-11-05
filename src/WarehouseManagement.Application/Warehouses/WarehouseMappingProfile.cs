using AutoMapper;
using WarehouseManagement.Application.Warehouses.Dtos;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Warehouses;

public class WarehouseMappingProfile : Profile
{
    public WarehouseMappingProfile()
    {
        CreateMap<Warehouse, WarehouseDto>();
    }
}
