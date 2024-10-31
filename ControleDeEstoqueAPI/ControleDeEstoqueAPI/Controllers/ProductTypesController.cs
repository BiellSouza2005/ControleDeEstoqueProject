using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Data.DTOs.ProductType;
using ControleDeEstoqueAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductTypesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() =>
            Ok(_context.ProductTypes.ToList());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _context.ProductTypes.Find(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost("AdicionarTipoDeProduto")]
        public async Task<IActionResult> Create([FromBody] ProductTypeDTO productTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productType = new ProductType
            {
                Name = productTypeDto.Name
            };

            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = productType.ProductTypeId }, productType);
        }

        [HttpPut("AlterarTipoDeProduto/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductTypeDTO productTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            productType.Name = productTypeDto.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ProductTypes.Any(e => e.ProductTypeId == id))
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

        [HttpDelete("DeletarTipoDeProduto/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productType = _context.ProductTypes.Find(id);
            if (productType == null) return NotFound();

            _context.ProductTypes.Remove(productType);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
