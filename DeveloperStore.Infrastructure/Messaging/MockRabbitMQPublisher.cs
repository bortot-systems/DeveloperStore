using DeveloperStore.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Infrastructure.Messaging
{
    public class MockRabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly ILogger<MockRabbitMQPublisher> _logger;
        public MockRabbitMQPublisher(ILogger<MockRabbitMQPublisher> logger)
        {
            _logger = logger;
        }
        public Task PublishAsync(string eventType, object message)
        {
            var saleId = message?.GetType().GetProperty("SaleId")?.GetValue(message)?.ToString();
            var logMessage = saleId != null
                ? $"[MQ] Event {eventType} performed for SaleId: {saleId}"
                : $"[MQ] Event {eventType} performed, {message}";

            Console.WriteLine(logMessage);
            _logger.LogInformation(logMessage);

            return Task.CompletedTask;
        }
    }
}
