using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Dto;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers
{
    [Authorize] // Requires authentication
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("GetOrderById")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound("Order not found.");
            return Ok(order);
        }
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequestDTO orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = new Order
            {
                ProductId = orderDto.ProductId,
                Quantity = orderDto.Quantity,
                Status = "Processing",
                CreatedAt = DateTime.UtcNow
            };

            var createdOrder = await _orderService.PlaceOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.OrderId }, createdOrder);
        }

        [HttpPut("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatusUpdateDTO statusDto)
        {
            var success = await _orderService.UpdateOrderStatusAsync(orderId, statusDto.Status);
            if (!success) return NotFound("Order not found.");

            return Ok("Order status updated.");
        }

        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var success = await _orderService.DeleteOrderAsync(orderId);
            if (!success) return NotFound("Order not found.");

            return Ok("Order deleted.");
        }
    }
}
