using OnlineShopNET.Domain.Entities;

namespace OnlineShopNET.Infrastructure.Persistence.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool> CreateOrder(Order2 order);
    }
}
