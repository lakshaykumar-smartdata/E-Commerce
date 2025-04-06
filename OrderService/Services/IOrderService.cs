
using OrderService.Models;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<bool> DeleteOrderAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<int> PlaceOrderAsync(Order order, string bearerToken);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }
}
