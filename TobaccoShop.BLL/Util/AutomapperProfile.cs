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
            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.Comments, opt => opt.Ignore());

            CreateMap<ClientProfile, UserDTO>()
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<OrderedProduct, OrderedProductDTO>()
                .ForMember(dest => dest.LinePrice, opt => opt.MapFrom(src => src.Product.Price * src.Quantity))
                .ForMember(dest => dest.MarkModel, opt => opt.MapFrom(src => src.Product.Mark + " " + src.Product.Model))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            CreateMap<OrderedProductDTO, OrderedProduct>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore());

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));
        }
    }
}
