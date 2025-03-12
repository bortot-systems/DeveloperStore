namespace DeveloperStore.Domain.Events
{
    public class SaleCreatedEvent : IDomainEvent
    {
        public Guid SaleId { get; }
        public DateTime OccurredOn { get; }

        public SaleCreatedEvent(Guid saleId)
        {
            SaleId = saleId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
