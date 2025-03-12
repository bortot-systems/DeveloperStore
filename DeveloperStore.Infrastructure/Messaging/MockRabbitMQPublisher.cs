using DeveloperStore.Application.Interfaces;

namespace DeveloperStore.Infrastructure.Messaging
{
    public class MockRabbitMQPublisher : IRabbitMQPublisher
    {
        public Task PublishAsync(string eventType, object message)
        {
            Console.WriteLine($"[MQ] Event {eventType} published with message: {message}");
            return Task.CompletedTask;
        }
    }
}
