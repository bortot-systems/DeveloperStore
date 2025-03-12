using DeveloperStore.Shared.DTOs;

namespace DeveloperStore.Application.Interfaces
{
    public interface ISaleService
    {
        Task<SaleDto> GetSaleByIdAsync(Guid saleId);
        Task<SaleDto> CreateSaleAsync(SaleDto saleDto);
        Task<bool> UpdateSaleAsync(Guid saleId, SaleDto saleDto);
        Task CancelSaleAsync(Guid saleId);
    }
}
