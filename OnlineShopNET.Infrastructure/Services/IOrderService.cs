using OnlineShopNET.Application.DTOs;

namespace OnlineShopNET.Infrastructure.Services
{
    public interface IOrderService
    {
        Task<CreateOrderDTO> PlaceOrder(int userId, List<ProductsListDTO> productlist);
    }
}
