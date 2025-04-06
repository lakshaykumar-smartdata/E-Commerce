using MassTransit;
using NotificationService.Services;
using Shared.Models;

namespace NotificationService.Consumer
{
    public class OrderShippedConsumer : IConsumer<OrderShipped>
    {
        private readonly IEmailService _emailService;

        public OrderShippedConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<OrderShipped> context)
        {
            var message = context.Message;

            Console.WriteLine($"[NotificationService] Sending shipment email to {message.CustomerEmail} for Order {message.OrderId}");

            var subject = $"Your order {message.OrderId} has been shipped!";
            var body = $@"
                <h3>Your order has shipped!</h3>
                <p>Order ID: <strong>{message.OrderId}</strong></p>
                <p>Tracking Number: <strong>{message.TrackingNumber}</strong></p>
                <p>Shipped At: {message.ShippedAt:G}</p>
                <p>Thank you for shopping with us.</p>
            ";

            await _emailService.SendEmailAsync(message.CustomerEmail, subject, body);
        }
    }
}
