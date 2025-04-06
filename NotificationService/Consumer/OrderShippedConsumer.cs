using MassTransit;
using Shared.Models;

namespace NotificationService.Consumer
{
    public class OrderShippedConsumer : IConsumer<OrderShipped>
    {
        public Task Consume(ConsumeContext<OrderShipped> context)
        {
            var message = context.Message;

            Console.WriteLine($"[Notification] Order {message.OrderId} has shipped to {message.CustomerEmail} with tracking number {message.TrackingNumber}");

            // Send shipment notification email or SMS here...

            return Task.CompletedTask;
        }
    }
}
