using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using System.Net.Http.Headers;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderServiceDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient, OrderServiceDbContext dbContext)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders.FindAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<int> PlaceOrderAsync(Order order, string bearerToken)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (string.IsNullOrWhiteSpace(bearerToken)) throw new ArgumentException("Bearer token is required", nameof(bearerToken));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            bool stockDeducted = await DeductStockAsync(order.ProductId, order.Quantity);
            if (!stockDeducted)
            {
                return 0;
            }

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order.OrderId;
        }

        private async Task<bool> DeductStockAsync(Guid productId, int quantity)
        {
            var requestUrl = $"https://localhost:44384/product/api/Product/DeductStock?id={productId}&quantity={quantity}";
            var response = await _httpClient.PutAsync(requestUrl, null);
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null) return false;

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
