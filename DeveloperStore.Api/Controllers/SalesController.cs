using DeveloperStore.Application.Interfaces;
using DeveloperStore.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet("sales")]
        public async Task<IActionResult> GetSales()
        {
            var sales = await _saleService.GetSalesAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var sale = await _saleService.GetSaleByIdAsync(id);
            if (sale == null) return NotFound();
            return Ok(sale);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaleDto saleDto)
        {
            var sale = await _saleService.CreateSaleAsync(saleDto);
            return CreatedAtAction(nameof(GetById), new { id = sale.SaleId }, sale);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, SaleDto saleDto)
        {
            var updated = await _saleService.UpdateSaleAsync(id, saleDto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _saleService.CancelSaleAsync(id);
            return NoContent();
        }
    }
}
