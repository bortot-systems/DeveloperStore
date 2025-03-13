using DeveloperStore.Application.Interfaces;
using DeveloperStore.Application.Services;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Shared.DTOs;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeveloperStore.Tests.Services
{
    public class SaleServiceTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<IRabbitMQPublisher> _rabbitMQPublisherMock;
        private readonly Mock<ILogger<SaleService>> _loggerMock;
        private readonly ISaleService _saleService;

        public SaleServiceTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _rabbitMQPublisherMock = new Mock<IRabbitMQPublisher>();
            _loggerMock = new Mock<ILogger<SaleService>>();
            _saleService = new SaleService(_saleRepositoryMock.Object, _rabbitMQPublisherMock.Object, _loggerMock.Object);
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

            // Verify logger
            _loggerMock.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Sale not found.")),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
        }
    }
}
