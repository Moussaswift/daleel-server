using Microsoft.AspNetCore.Mvc;
using daleel.Entities;
using daleel.Services;
using Microsoft.EntityFrameworkCore;
namespace daleel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        // GET: api/Sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(Guid id)
        {
            var sale = await _saleService.GetSaleAsync(id);

            if (sale == null)
            {
                return NotFound();
            }

            return sale;
        }

        // POST: api/Sales
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(Sale sale)
        {
            var createdSale = await _saleService.CreateSaleAsync(sale);
            return CreatedAtAction(nameof(GetSale), new { id = createdSale.Id }, createdSale);
        }

        // PUT: api/Sales/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(Guid id, Sale sale)
        {
            if (id != sale.Id)
            {
                return BadRequest();
            }

            try
            {
                await _saleService.UpdateSaleAsync(sale);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SaleExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(Guid id)
        {
            var sale = await _saleService.GetSaleAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            await _saleService.DeleteSaleAsync(id);

            return NoContent();
        }

        private async Task<bool> SaleExistsAsync(Guid id)
        {
            var sale = await _saleService.GetSaleAsync(id);
            return sale != null;
        }
    }
}
