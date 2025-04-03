using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dto;
using ProductService.Models;
using System.Net.Http;

namespace ProductService.Services
{
    public class ProductService: IProductService
    {
        private readonly ProductServiceDbContext _dbContext;

        public ProductService(ProductServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> CreateOrUpdateProduct(ProductCreateRequestDTO dto)
        {
            // Validate SellerId with UserService API
            bool isValidSeller = await ValidateSeller(dto.SellerId);
            if (!isValidSeller)
            {
                return Guid.Empty;
            }

            Product product;

            if (dto.ProductId == Guid.Empty)
            {
                // Create new product
                product = new Product
                {
                    ProductId = Guid.NewGuid(),
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    Stock = dto.Stock,
                    SellerId = dto.SellerId
                };

                _dbContext.Products.Add(product);
            }
            else
            {
                // Update existing product
                product = await _dbContext.Products.FindAsync(dto.ProductId);
                if (product == null)
                {
                    return Guid.Empty;
                }

                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.Stock = dto.Stock;
            }

            await _dbContext.SaveChangesAsync();
            return product.ProductId;
        }
        private async Task<bool> ValidateSeller(int sellerId)
        {
            //need httpcall to userservice
            return true;
        }
        public async Task<List<Product>> GetProducts(int sellerId)
        {
            if (sellerId > 0)
            {
                return await _dbContext.Products
                    .Where(p => p.SellerId == sellerId)
                    .ToListAsync();
            }
            else
            {
                return await _dbContext.Products
                    .ToListAsync();
            }
        }

        public async Task<bool> DeleteProductAsync(int sellerId, Guid productId)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.SellerId == sellerId && p.ProductId == productId);

            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> DeductStockAsync(Guid productId, int quantity)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
                return false; // Product not found

            if (product.Stock < quantity)
                return false; // Not enough stock

            product.Stock -= quantity;
            await _dbContext.SaveChangesAsync();
            return true;
        }


    }
}
