using AutoMapper;
using ProductApp.Infrastructure.Data.Configurations;
using ProductApp.Application.DTOS;

namespace ProductApp.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
