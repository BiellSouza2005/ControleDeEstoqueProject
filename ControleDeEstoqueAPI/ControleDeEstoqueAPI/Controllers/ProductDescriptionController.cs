using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Data.DTOs.ProductDescription;
using ControleDeEstoqueAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductDesciptionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductDesciptionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("VerDescricoes")]
        public IActionResult GetAll() =>
            Ok(_context.ProductDescriptions.ToList());

        [HttpGet("BuscarDescricao/{id}")]
        public IActionResult GetById(int id)
        {
            var productDescription = _context.ProductDescriptions.Find(id);
            return productDescription == null ? NotFound() : Ok(productDescription);
        }

        [HttpPost("AdicionarDescricaoAoProduto")]
        public async Task<IActionResult> Create([FromBody] ProductDescriptionDTO productDescriptionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productDescription = new ProductDescription
            {
                ProductId = productDescriptionDto.ProductId,
                Description = productDescriptionDto.Description
            };

            _context.ProductDescriptions.Add(productDescription);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = productDescription.ProductId }, productDescription);
        }

        [HttpPut("AlterarDescricaoDoProduto/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDescriptionDTO productDescriptionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productDescription = await _context.ProductDescriptions.FindAsync(id);
            if (productDescription == null)
            {
                return NotFound();
            }

            productDescription.ProductId = productDescriptionDto.ProductId;
            productDescription.Description = productDescriptionDto.Description;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ProductDescriptions.Any(e => e.ProductId == id))
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

        [HttpDelete("DeletarDescrição/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productDescription = _context.ProductDescriptions.Find(id);
            if (productDescription == null) return NotFound();

            _context.ProductDescriptions.Remove(productDescription);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
