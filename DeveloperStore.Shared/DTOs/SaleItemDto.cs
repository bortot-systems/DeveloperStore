namespace DeveloperStore.Shared.DTOs
{
    public class SaleItemDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
