using Microsoft.EntityFrameworkCore;
using OnlineShopNET.Application.DTOs;
using OnlineShopNET.Domain.Entities;
using OnlineShopNET.Infrastructure.Data;
using OnlineShopNET.Infrastructure.Persistence.Interfaces;

namespace OnlineShopNET.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OnlineShopDbContext _dbContext;
        public ProductRepository(OnlineShopDbContext dbContext)
        {
                _dbContext = dbContext;
        }

        public async Task<List<Product>> GetAvailableProducts()
        {
            return await _dbContext.Product.Where(x => x.product_status == true).ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            var productResult = await _dbContext.Product.FirstOrDefaultAsync(x => x.productId == id && x.product_status == true);
            if (productResult == null)
                return null;
            return productResult;
            
        }
        public async Task<Product> CreateProduct (Product product)
        {
            await _dbContext.Product.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
        public async Task<Product> UpdateProduct(Product product)
        {
            if (product.productId == 0)
                return null;
            var checkProduct = await _dbContext.Product.FirstOrDefaultAsync(x => x.productId == product.productId);
            if (checkProduct == null)
                return null;
            checkProduct.productId = product.productId;
            checkProduct.product_name = product.product_name;
            checkProduct.product_description = product.product_description;
            checkProduct.product_price = product.product_price;
            checkProduct.product_quantity = product.product_quantity;
            checkProduct.categoryId = product.categoryId;
            checkProduct.product_status = product.product_status;
            _dbContext.Product.Update(checkProduct);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            var productResult = await _dbContext.Product.FirstOrDefaultAsync(x => x.productId == product.productId);
            if (productResult == null)
                return false;
             _dbContext.Product.Remove(productResult);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantity(UpdateProductQuantity productQuantity)
        {
            var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.productId == productQuantity.productID);
            if (product == null)
                return false;
            product.product_quantity = productQuantity.product_quantity;
            if (product.product_quantity == 0)
                product.product_status = false;
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
