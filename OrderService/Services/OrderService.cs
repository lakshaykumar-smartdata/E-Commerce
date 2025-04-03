using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

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

        public async Task<Order> PlaceOrderAsync(Order order)
        {
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
