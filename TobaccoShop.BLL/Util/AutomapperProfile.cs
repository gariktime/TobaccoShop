using AutoMapper;
using TobaccoShop.BLL.DTO;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Entities.Identity;
using TobaccoShop.DAL.Entities.Products;

namespace TobaccoShop.BLL.Util
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Product, ProductDTO>();

            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.Comments, opt => opt.Ignore());

            CreateMap<ClientProfile, UserDTO>()
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(p => p.Orders))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<OrderedProduct, OrderedProductDTO>()
                .ForMember(dest => dest.LinePrice, opt => opt.Ignore())
                .ForMember(dest => dest.MarkModel, opt => opt.MapFrom(p => p.Product.Mark + " " + p.Product.Model))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(p => p.Product));

            CreateMap<OrderedProductDTO, OrderedProduct>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore());
        }
    }
}
