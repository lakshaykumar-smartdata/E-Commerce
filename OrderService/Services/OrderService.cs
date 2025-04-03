using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using System.Net.Http.Headers;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderServiceDbContext _dbContext;

        public OrderService(OrderServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders.FindAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<Order> PlaceOrderAsync(Order order, string bearerToken)
        {
            using var httpClient = new HttpClient();

            // Add Bearer Token to the request headers
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var productId = order.ProductId;
            var quantity = order.Quantity;
            var requestUrl = $"https://localhost:44384/product/api/Product/DeductStock?id={productId}&quantity={quantity}";

            var response = await httpClient.PutAsync(requestUrl, null);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to deduct stock for Product ID: {productId}");
            }

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
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
