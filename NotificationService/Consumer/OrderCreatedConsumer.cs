using MassTransit;
using Shared.Models;

namespace NotificationService.Consumer
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        public Task Consume(ConsumeContext<OrderCreated> context)
        {
            var message = context.Message;

            Console.WriteLine($"[NotificationService] Send email to {message.CustomerEmail} for Order {message.OrderId}");

            // Add logic to send email...

            return Task.CompletedTask;
        }
    }
}
