namespace DeveloperStore.Shared.DTOs
{
    public class SaleDto
    {
        public Guid SaleId { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerId { get; set; }
        public string Branch { get; set; }
        public bool IsCancelled { get; set; }
        public decimal TotalSaleAmount { get; set; }
        public List<SaleItemDto> Items { get; set; }
    }

}
