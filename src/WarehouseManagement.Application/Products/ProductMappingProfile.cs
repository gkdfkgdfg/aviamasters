using AutoMapper;
using WarehouseManagement.Application.Products.Dtos;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Products;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
    }
}
