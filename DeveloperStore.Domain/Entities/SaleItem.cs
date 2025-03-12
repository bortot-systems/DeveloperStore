using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Entities
{
    public class SaleItem
    {
        public Guid SaleItemId { get; private set; }
        public Guid SaleId { get; private set; }
        public ProductReference Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalAmount => Quantity * UnitPrice - Discount;

        private SaleItem() { }

        public SaleItem(ProductReference product, int quantity, decimal unitPrice, out string errorMessage)
        {
            Product = product;
            Quantity = quantity;
            UnitPrice = unitPrice;
            errorMessage = string.Empty;

            if (quantity > 20)
            {
                errorMessage = "Cannot sell more than 20 identical items.";
                return;
            }

            Discount = (quantity >= 4 && quantity < 10)
                ? unitPrice * 0.10m * quantity
                : (quantity >= 10 && quantity <= 20)
                    ? unitPrice * 0.20m * quantity
                    : 0m;

        }
    }
}
