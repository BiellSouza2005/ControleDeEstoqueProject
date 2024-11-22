using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Data.DTOs.ProductDescription;
using ControleDeEstoqueAPI.Data.DTOs.User;
using ControleDeEstoqueAPI.Entities;
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

        [HttpGet("VerDescricaoDoProdutoPor/{id}")]
        public async Task<ActionResult<ProductDescriptionDTO>> GetProductDescriptionById(int id)
        {
            var productDescription = await _context.ProductDescriptions
                .Where(pd => pd.Id == id && !pd.IsActive) // Filtra usuários com IsActive = false
                .FirstOrDefaultAsync();

            if (productDescription == null)
            {
                return NotFound("Descrição do Produto não encontrado ou não está inativo.");
            }

            var productDescriptionDto = new ProductDescriptionDTO
            {
                ProductDescriptionId = productDescription.Id,
                ProductId = productDescription.ProductId,
                Description = productDescription.Description
            };

            return Ok(productDescriptionDto);
        }

        [HttpGet("VerTodasDescricoesDeProdutos")]
        public async Task<ActionResult<IEnumerable<ProductDescriptionDTO>>> GetAllProductDescriptions()
        {
            var productDescriptions = await _context.ProductDescriptions
                .Where(pd => !pd.IsActive) // Filtra apenas usuários com IsActive = false
                .Select(pd => new ProductDescriptionDTO
                {
                    ProductDescriptionId = pd.Id,
                    ProductId = pd.ProductId,
                    Description = pd.Description
                })
                .ToListAsync();

            if (!productDescriptions.Any())
            {
                return NotFound("Nenhuma descrição de produto inativa encontrada.");
            }

            return Ok(productDescriptions);
        }

        [HttpPost("AdicionarDescricaoAoProduto")]
        public async Task<IActionResult> Create([FromBody] ProductDescriptionDTO productDescriptionDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productDescription = new ProductDescription
            {
                ProductId = productDescriptionDto.ProductId,
                Description = productDescriptionDto.Description,
                UserInclusion = userInclusion,
                UserChange = userInclusion
            };

            _context.ProductDescriptions.Add(productDescription);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductDescriptionById), new { id = productDescription.Id }, productDescriptionDto);
        }

        [HttpPut("AlterarDescricaoDoProduto/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDescriptionDTO productDescriptionDto, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            if (id != productDescriptionDto.ProductDescriptionId)
            {
                return BadRequest("ID da descrição do produto não corresponde.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProductDescription = await _context.ProductDescriptions.FindAsync(id);

            if (existingProductDescription == null)
            {
                return NotFound("Descrição do produto não encontrado.");
            }

            existingProductDescription.ProductId = productDescriptionDto.ProductId;
            existingProductDescription.Description = productDescriptionDto.Description;
            existingProductDescription.DateTimeChange = DateTime.UtcNow;
            existingProductDescription.UserChange = userChange;

            _context.ProductDescriptions.Update(existingProductDescription);
            await _context.SaveChangesAsync();

            return Ok("Descrição do produto atualizado com sucesso.");
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

        [HttpDelete("DesativarUsuário/{id}")]
        public async Task<IActionResult> Disable(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var productDescription = await _context.ProductDescriptions.FindAsync(id);

            if (productDescription == null)
            {
                return NotFound($"Descrição do produto com ID {id} não encontrado.");
            }

            productDescription.IsActive = true;
            productDescription.DateTimeChange = DateTime.UtcNow;
            productDescription.UserChange = userChange;

            _context.ProductDescriptions.Update(productDescription);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
