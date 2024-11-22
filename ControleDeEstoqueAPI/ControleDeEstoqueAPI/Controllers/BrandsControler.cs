using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.ProductType;
using ControleDeEstoqueAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoqueAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("VerMarcasPor/{id}")]
        public async Task<ActionResult<BrandDTO>> GetBrandsById(int id)
        {
            var brand = await _context.Brands
                .Where(b => b.Id == id && !b.IsActive) // Filtra usuários com IsActive = false
                .FirstOrDefaultAsync();

            if (brand == null)
            {
                return NotFound("Marca não encontrada ou não está inativa.");
            }

            var brandsDTO = new BrandDTO
            {
                BrandId = brand.Id,
                Name = brand.Name
            };

            return Ok(brandsDTO);
        }

        [HttpGet("VerTodosOsTiposDeProduto")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var brand = await _context.Brands
                .Where(b => !b.IsActive) // Filtra apenas usuários com IsActive = false
                .Select(b => new BrandDTO
                {
                    BrandId = b.Id,
                    Name = b.Name
                })
                .ToListAsync();

            if (!brand.Any())
            {
                return NotFound("Nenhum tipo de produto inativo encontrado.");
            }

            return Ok(brand);
        }

        [HttpPost("AdicionaMarca")]
        public async Task<IActionResult> AdicionaMarca([FromBody] BrandDTO brandDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            if (string.IsNullOrEmpty(brandDto.Name))
            {
                return BadRequest("O nome da marca é obrigatório.");
            }

            var existingBrand = _context.Brands.FirstOrDefault(b => b.Name == brandDto.Name);
            if (existingBrand != null)
            {
                return Conflict("A marca já existe.");
            }

            var brand = new Brand
            {
                Name = brandDto.Name,
                UserInclusion = userInclusion,
                UserChange = userInclusion
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AdicionaMarca), new { id = brand.Id }, brand);
        }

        [HttpPut("MudaMarca/{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandDTO brandDTO, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            if (id != brandDTO.BrandId)
            {
                return BadRequest("ID da marca não corresponde.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBrand = await _context.Brands.FindAsync(id);

            if (existingBrand == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            existingBrand.Name = brandDTO.Name;
            existingBrand.DateTimeChange = DateTime.UtcNow;
            existingBrand.UserChange = userChange;

            _context.Brands.Update(existingBrand);
            await _context.SaveChangesAsync();

            return Ok("Marca atualizada com sucesso.");
        }

        [HttpDelete("DeletaMarca/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null) return NotFound();

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("DesativarMarca/{id}")]
        public async Task<IActionResult> Disable(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound($"Marca com ID {id} não encontrado.");
            }

            brand.IsActive = true;
            brand.DateTimeChange = DateTime.UtcNow;
            brand.UserChange = userChange;

            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
