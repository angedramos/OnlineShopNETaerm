using OnlineShopNET.Domain.Entities;
using OnlineShopNET.Infrastructure.Data;
using OnlineShopNET.Infrastructure.Persistence.Interfaces;

namespace OnlineShopNET.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OnlineShopDbContext _dbContext;
        public OrderRepository(OnlineShopDbContext dbContext)
        {
                _dbContext = dbContext;
        }

        public async Task<bool> CreateOrder(Order2 order)
        {
            await _dbContext.Orders2.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
