using DeveloperStore.Application.Interfaces;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Domain.ValueObjects;
using DeveloperStore.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DeveloperStore.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public SaleService(ISaleRepository saleRepository, IRabbitMQPublisher rabbitMQPublisher)
        {
            _saleRepository = saleRepository;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public async Task<SaleDto> GetSaleByIdAsync(Guid saleId)
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);
            if (sale == null) return null;

            return new SaleDto
            {
                SaleId = sale.SaleId,
                SaleNumber = sale.SaleNumber,
                SaleDate = sale.SaleDate,
                CustomerId = sale.Customer.CustomerId.ToString(),
                Branch = sale.Branch,
                IsCancelled = sale.IsCancelled,
                TotalSaleAmount = sale.Items.Sum(i => (i.Quantity * i.UnitPrice) - i.Discount),
                Items = sale.Items.Select(i => new SaleItemDto
                {
                    ProductId = i.Product.ProductId.ToString(),
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    TotalAmount = (i.Quantity * i.UnitPrice) - i.Discount
                }).ToList()
            };
        }


        public async Task<SaleDto> CreateSaleAsync(SaleDto saleDto)
        {
            var customerReference = new CustomerReference(
                Guid.Parse(saleDto.CustomerId),
                "Customer Name",
                "Customer Email"
            );

            var sale = new Sale(saleDto.SaleNumber, customerReference, saleDto.Branch);

            foreach (var itemDto in saleDto.Items)
            {
                var productReference = new ProductReference(
                    Guid.Parse(itemDto.ProductId),
                    "Product Name"
                );

                var saleItem = new SaleItem(productReference, itemDto.Quantity, itemDto.UnitPrice, out var errorMessage);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    throw new ValidationException(errorMessage);
                }

                sale.AddItem(saleItem);
            }

            await _saleRepository.AddAsync(sale);

            await _rabbitMQPublisher.PublishAsync("SaleCreated", sale);

            return new SaleDto
            {
                SaleId = sale.SaleId,
                SaleNumber = sale.SaleNumber,
                SaleDate = sale.SaleDate,
                CustomerId = sale.Customer.CustomerId.ToString(),
                Branch = sale.Branch,
                IsCancelled = sale.IsCancelled,
                TotalSaleAmount = sale.Items.Sum(i => (i.Quantity * i.UnitPrice) - i.Discount),
                Items = sale.Items.Select(item => new SaleItemDto
                {
                    ProductId = item.Product.ProductId.ToString(),
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    TotalAmount = item.TotalAmount
                }).ToList()
            };
        }


        public async Task<bool> UpdateSaleAsync(Guid saleId, SaleDto saleDto)
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);
            if (sale == null) return false;

            sale.UpdateDetails(
                saleDto.SaleNumber,
                new CustomerReference(
                    Guid.Parse(saleDto.CustomerId),
                    "Customer Name",
                    "Customer Email"
                ),
                saleDto.Branch
            );

            sale.ClearItems();


            foreach (var itemDto in saleDto.Items)
            {
                var productReference = new ProductReference(
                    Guid.Parse(itemDto.ProductId),
                    "Updated Product Name"
                );

                var saleItem = new SaleItem(productReference, itemDto.Quantity, itemDto.UnitPrice, out var errorMessage);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    throw new ValidationException(errorMessage);
                }
                sale.AddItem(saleItem);
            }

            await _saleRepository.UpdateAsync(sale);

            await _rabbitMQPublisher.PublishAsync("SaleModified", sale);

            return true;
        }

        public async Task CancelSaleAsync(Guid saleId)
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);
            if (sale == null) throw new Exception("Sale not found.");

            sale.Cancel();

            await _saleRepository.UpdateAsync(sale);

            await _rabbitMQPublisher.PublishAsync("SaleCancelled", sale);
        }
    }
}

