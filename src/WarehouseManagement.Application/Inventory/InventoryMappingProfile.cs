using AutoMapper;
using WarehouseManagement.Application.Inventory.Dtos;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Inventory;

public class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        CreateMap<InboundOrderLine, InboundOrderLineDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));

        CreateMap<InboundOrder, InboundOrderDto>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : string.Empty))
            .ForMember(dest => dest.Lines, opt => opt.MapFrom(src => src.Lines));
    }
}
