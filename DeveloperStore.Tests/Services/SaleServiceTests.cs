using DeveloperStore.Application.Interfaces;
using DeveloperStore.Application.Services;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Shared.DTOs;
using Moq;

namespace DeveloperStore.Tests.Services
{
    public class SaleServiceTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<IRabbitMQPublisher> _rabbitMQPublisherMock;
        private readonly ISaleService _saleService;

        public SaleServiceTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _rabbitMQPublisherMock = new Mock<IRabbitMQPublisher>();
            _saleService = new SaleService(_saleRepositoryMock.Object, _rabbitMQPublisherMock.Object);
        }

        [Fact]
        public async Task CreateSaleAsync_ShouldPublishEvent_WhenSaleIsCreated()
        {
            // Arrange
            var saleDto = new SaleDto
            {
                SaleNumber = "12345",
                CustomerId = Guid.NewGuid().ToString(),
                Branch = "Main Branch",
                Items = new List<SaleItemDto>
                {
                    new SaleItemDto
                    {
                        ProductId = Guid.NewGuid().ToString(),
                        Quantity = 5,
                        UnitPrice = 100m
                    }
                }
            };

            // Act
            var sale = await _saleService.CreateSaleAsync(saleDto);

            // Assert
            _saleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Sale>()), Times.Once);
            _rabbitMQPublisherMock.Verify(x => x.PublishAsync("SaleCreated", It.IsAny<Sale>()), Times.Once);
        }

        [Fact]
        public async Task CancelSaleAsync_ShouldThrowException_WhenSaleNotFound()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            _saleRepositoryMock.Setup(x => x.GetByIdAsync(saleId))
                .ReturnsAsync((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _saleService.CancelSaleAsync(saleId));
        }
    }
}
