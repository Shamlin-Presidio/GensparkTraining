using AutoMapper;
using ShopApi.Dtos;
using ShopApi.Models;

namespace ShopApi.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<News, NewsDto>().ReverseMap();
            CreateMap<News, NewsResponseDto>();
            CreateMap<NewsCreateDto, News>();

            CreateMap<Product, ProductResponseDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductDetailDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<User, UserResponseDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();

            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.OrderedAt, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
                src.OrderDetails.Sum(d => d.Quantity * d.UnitPrice)
            ));
    
        }
    }
}
