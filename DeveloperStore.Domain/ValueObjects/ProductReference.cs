namespace DeveloperStore.Domain.ValueObjects
{
    public class ProductReference
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }

        private ProductReference() { }

        public ProductReference(Guid productId, string productName)
        {
            ProductId = productId;
            ProductName = productName;
        }
    }
}
