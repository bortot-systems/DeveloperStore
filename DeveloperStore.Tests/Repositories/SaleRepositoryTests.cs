using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.ValueObjects;
using DeveloperStore.Infrastructure.Persistence;
using DeveloperStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Tests.Repositories
{
    public class SaleRepositoryTests
    {
        private readonly SalesDbContext _context;
        private readonly SaleRepository _saleRepository;

        public SaleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<SalesDbContext>()
                .UseInMemoryDatabase(databaseName: "SalesTestDb")
                .Options;

            _context = new SalesDbContext(options);
            _saleRepository = new SaleRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddSale_ToDatabase()
        {
            // Arrange
            var sale = new Sale("12345", new CustomerReference(Guid.NewGuid(), "Jane Doe", "jane@example.com"), "Main Branch");

            // Act
            await _saleRepository.AddAsync(sale);

            // Assert
            var savedSale = await _context.Sales.FindAsync(sale.SaleId);
            Assert.NotNull(savedSale);
            Assert.Equal("12345", savedSale.SaleNumber);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSale_WhenSaleExists()
        {
            // Arrange
            var sale = new Sale("12345", new CustomerReference(Guid.NewGuid(), "Jane Doe", "jane@example.com"), "Branch A");
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            // Act
            var retrievedSale = await _saleRepository.GetByIdAsync(sale.SaleId);

            // Assert
            Assert.NotNull(retrievedSale);
            Assert.Equal("12345", retrievedSale.SaleNumber);
        }
    }
}
