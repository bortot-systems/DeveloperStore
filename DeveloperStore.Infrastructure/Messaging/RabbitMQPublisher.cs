using DeveloperStore.Application.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;

namespace DeveloperStore.Infrastructure.Messaging
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IConnection _connection;

        public RabbitMQPublisher(IConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task PublishAsync(string eventType, object message)
        {
            if (_connection == null || !_connection.IsOpen)
                throw new InvalidOperationException("RabbitMQ connection is not open.");

            try
            {
                using var channel = await _connection.CreateChannelAsync();

                await channel.ExchangeDeclareAsync(exchange: "SalesExchange", type: ExchangeType.Topic);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                await channel.BasicPublishAsync(exchange: "SalesExchange", routingKey: eventType, body: body);

                Console.WriteLine($"Event {eventType} published.");
            }
            catch (BrokerUnreachableException ex)
            {
                Console.WriteLine($"RabbitMQ broker is unreachable: {ex.Message}");
                throw;
            }
        }
    }
}
