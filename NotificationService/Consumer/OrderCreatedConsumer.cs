using MassTransit;
using NotificationService.Services;
using Shared.Models;

namespace NotificationService.Consumer
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly IEmailService _emailService;

        public OrderCreatedConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            var message = context.Message;

            Console.WriteLine($"[NotificationService] Sending order confirmation to {message.CustomerEmail} for Order ID: {message.OrderId}");

            var subject = $"Order Confirmation - Order #{message.OrderId}";
            var body = $@"
                <h3>Thank you for your order!</h3>
                <p>Your order <strong>{message.OrderId}</strong> has been successfully placed.</p>
                <p>We’ll notify you when it ships.</p>
            ";

            await _emailService.SendEmailAsync(message.CustomerEmail, subject, body);
        }
    }
}
