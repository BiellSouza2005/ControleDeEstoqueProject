using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("VerProdutos")]
        public IActionResult GetAll() =>
            Ok(_context.Products.ToList());

        [HttpGet("BuscarProduto/{id}")]
        public IActionResult GetById(int id)
        {
            var product = _context.Products.Find(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost("AdicionarProduto")]
        public async Task<IActionResult> Create([FromBody] ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                BrandId = productDto.BrandId,
                ProductTypeId = productDto.ProductTypeId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
        }

        [HttpPut("AlterarProduto/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = productDto.Name;
            product.Price = productDto.Price;  
            product.BrandId = productDto.BrandId;
            product.ProductTypeId = productDto.ProductTypeId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductId == id))
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

        [HttpPut("AdicionarQuantidade/{id}/{quantidade}")]
        public async Task<IActionResult> AddQuantity(int id, int quantidade)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound("Produto não encontrado.");

            product.Quantity += quantidade;
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut("SubtrairQuantidade/{id}/{quantidade}")]
        public async Task<IActionResult> SubtractQuantity(int id, int quantidade)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound("Produto não encontrado.");

            if (product.Quantity < quantidade)
            {
                return BadRequest("Quantidade insuficiente no estoque.");
            }

            product.Quantity -= quantidade;
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("DeletarProduto/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
