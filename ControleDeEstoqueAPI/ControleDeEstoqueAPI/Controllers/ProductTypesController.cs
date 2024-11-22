using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Data.DTOs.ProductType;
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
    public class ProductTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductTypesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("VerTiposDeProdutosPor/{id}")]
        public async Task<ActionResult<ProductTypeDTO>> GetProductTypeById(int id)
        {
            var productType = await _context.ProductTypes
                .Where(u => u.Id == id && !u.IsActive) // Filtra usuários com IsActive = false
                .FirstOrDefaultAsync();

            if (productType == null)
            {
                return NotFound("Tipo de Produto não encontrado ou não está inativo.");
            }

            var productTyoDto = new ProductTypeDTO
            {
                ProductTypeId = productType.Id,
                Name = productType.Name
            };

            return Ok(productTyoDto);
        }

        [HttpGet("VerTodosOsTiposDeProduto")]
        public async Task<ActionResult<IEnumerable<ProductTypeDTO>>> GetAllProductTypes()
        {
            var productType = await _context.ProductTypes
                .Where(pt => !pt.IsActive) // Filtra apenas usuários com IsActive = false
                .Select(pt => new ProductTypeDTO
                {
                    ProductTypeId = pt.Id,
                    Name = pt.Name
                })
                .ToListAsync();

            if (!productType.Any())
            {
                return NotFound("Nenhum tipo de produto inativo encontrado.");
            }

            return Ok(productType);
        }

        [HttpPost("AdicionarTipoDeProduto")]
        public async Task<IActionResult> Create([FromBody] ProductTypeDTO productTypeDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productType = new ProductType
            {
                Name = productTypeDto.Name,
                UserInclusion = userInclusion,
                UserChange = userInclusion
            };

            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductTypeById), new { id = productType.Id }, productType);
        }

        [HttpPut("AlterarTipoDeProduto/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductTypeDTO productTypeDto, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            if (id != productTypeDto.ProductTypeId)
            {
                return BadRequest("ID do tipo de produto não corresponde.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProductType = await _context.ProductTypes.FindAsync(id);

            if (existingProductType == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            existingProductType.Name = productTypeDto.Name;
            existingProductType.DateTimeChange = DateTime.UtcNow;
            existingProductType.UserChange = userChange;

            _context.ProductTypes.Update(existingProductType);
            await _context.SaveChangesAsync();

            return Ok("Tipo de Produto atualizado com sucesso.");
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

        [HttpDelete("DesativarTipoDeProduto/{id}")]
        public async Task<IActionResult> Disable(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var productType = await _context.ProductTypes.FindAsync(id);

            if (productType == null)
            {
                return NotFound($"Tipo de Produto com ID {id} não encontrado.");
            }

            productType.IsActive = true;
            productType.DateTimeChange = DateTime.UtcNow;
            productType.UserChange = userChange;

            _context.ProductTypes.Update(productType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
