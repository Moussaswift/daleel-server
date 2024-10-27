using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using daleel.Data;
using daleel.Entities;
using daleel.Models;
using Microsoft.EntityFrameworkCore;

namespace daleel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            });
        }

        // Add more actions (PUT, DELETE) as needed
    }
}
