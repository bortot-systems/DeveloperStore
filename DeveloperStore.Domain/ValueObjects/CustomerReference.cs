namespace DeveloperStore.Domain.ValueObjects
{
    public class CustomerReference
    {
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public string Email { get; private set; }

        private CustomerReference() { }

        public CustomerReference(Guid customerId, string customerName, string email)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            Email = email;
        }
    }
}
