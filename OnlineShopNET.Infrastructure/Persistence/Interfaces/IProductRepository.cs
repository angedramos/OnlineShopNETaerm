using OnlineShopNET.Application.DTOs;
using OnlineShopNET.Domain.Entities;

namespace OnlineShopNET.Infrastructure.Persistence.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAvailableProducts();
        Task<Product> GetProductById(int id);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<bool> DeleteProduct(Product product);
        Task<bool> UpdateQuantity(UpdateProductQuantity productQuantity);
    }
}
