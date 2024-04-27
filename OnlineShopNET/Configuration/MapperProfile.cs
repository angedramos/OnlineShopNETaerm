using AutoMapper;
using OnlineShopNET.Application.DTOs;
using OnlineShopNET.Domain.Entities;


namespace OnlineShopNET.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region Product
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductQuantity>().ReverseMap();
            #endregion

            #region User
            CreateMap<User,UserDTO>().ReverseMap();
            #endregion

            #region Order
            CreateMap<Order2, CreateOrderDTO>();
            #endregion
        }
    }
}
