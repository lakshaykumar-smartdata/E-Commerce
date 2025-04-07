using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Dto;
using OrderService.Models;
using OrderService.Services;
using Shared.Models;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(IOrderService orderService, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _publishEndpoint = publishEndpoint;
        }
        [Authorize(Policy = "CustomerOnly")]
        [HttpGet("GetOrderById")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound("Order not found.");
            return Ok(order);
        }
        [Authorize(Policy = "CustomerOnly")]
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        [Authorize(Policy = "CustomerOnly")]
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequestDTO orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bearerToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(bearerToken))
            {
                return Unauthorized("Bearer token is missing.");
            }

            var order = new Order
            {
                ProductId = orderDto.ProductId,
                CustomerId = orderDto.CustomerId,
                Quantity = orderDto.Quantity,
                Status = "Processing",
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                var orderId = await _orderService.PlaceOrderAsync(order, bearerToken);
                if (orderId > 0)
                {
                    await _publishEndpoint.Publish(new OrderCreated
                    {
                        OrderId = orderId,
                        CustomerEmail = orderDto.CustomerEmail,
                        SellerEmail = ""
                    });
                    return Ok(orderId);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error placing order: {ex.Message}");
            }
        }
        [Authorize(Policy = "SellerOnly")]
        [HttpPut("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatusUpdateDTO statusDto)
        {
            var shippedAt = DateTime.UtcNow;
            var trackingNumber = $"TRACK-{orderId}-{shippedAt:yyyyMMddHHmmss}";

            var success = await _orderService.UpdateOrderStatusAsync(orderId, statusDto.Status);

            if (!success)
                return NotFound("Order not found.");

            await _publishEndpoint.Publish(new OrderShipped
            {
                OrderId = orderId,
                ShippedAt = shippedAt,
                TrackingNumber = trackingNumber,
                CustomerEmail = statusDto.CustomerEmail
            });

            return Ok($"Order status updated. Tracking Number: {trackingNumber}");
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
