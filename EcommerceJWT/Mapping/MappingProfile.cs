using AutoMapper;
using Models.DTOs;
using Entity= DataAccess.Entity;
namespace EcommerceJWT.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Entity.Order, OrderResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(p => p.Name)));
            CreateMap<OrderResponse,Entity. Order>();
            CreateMap<UserResponse, Entity.AppUser>().ReverseMap();
            CreateMap<CreateOrder, Entity.Order>();
            CreateMap<CreateProduct, Entity.Product>();
            CreateMap<ProductResponse,Entity. Product>().ReverseMap();
            CreateMap<UpdateUser, Entity.AppUser>();

        }
    }
}
