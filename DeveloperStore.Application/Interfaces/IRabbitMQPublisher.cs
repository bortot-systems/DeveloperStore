namespace DeveloperStore.Application.Interfaces
{
    public interface IRabbitMQPublisher
    {
        Task PublishAsync(string eventType, object message);
    }
}
