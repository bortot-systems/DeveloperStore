using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task AddAsync(Sale sale);
        Task<Sale> GetByIdAsync(Guid id);
        Task UpdateAsync(Sale sale);
    }
}
