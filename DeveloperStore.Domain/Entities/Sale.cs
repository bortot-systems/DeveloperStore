using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Entities
{
    public class Sale
    {
        public Guid SaleId { get; private set; }
        public string SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public CustomerReference Customer { get; private set; }
        public string Branch { get; private set; }
        public bool IsCancelled { get; private set; }
        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
        public decimal TotalAmount => _items.Sum(item => item.TotalAmount);

        private Sale() { }

        public Sale(string saleNumber, CustomerReference customer, string branch)
        {
            SaleId = Guid.NewGuid();
            SaleNumber = saleNumber;
            SaleDate = DateTime.UtcNow;
            Customer = customer;
            Branch = branch;
        }

        public void AddItem(SaleItem item)
        {
            if (item.Quantity > 20)
                throw new Exception("Cannot sell more than 20 identical items.");

            _items.Add(item);
        }

        public void Cancel()
        {
            IsCancelled = true;
        }

        public void ClearItems()
        {
            _items.Clear();
        }

        public void UpdateDetails(string saleNumber, CustomerReference customer, string branch)
        {
            SaleNumber = saleNumber;
            Customer = customer;
            Branch = branch;
        }
    }
}
