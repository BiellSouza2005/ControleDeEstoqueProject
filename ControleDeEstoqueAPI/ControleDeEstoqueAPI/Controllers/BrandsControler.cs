using ControleDeEstoqueAPI.Data.DTOs.Brand;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _repository;

        public BrandsController(IBrandRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("AdicionaMarca")]
        public async Task<IActionResult> AddBrand([FromBody] BrandDTO brandDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            if (string.IsNullOrEmpty(brandDto.Name))
            {
                return BadRequest("O nome da marca é obrigatório.");
            }

            var createdBrand = await _repository.AddBrandAsync(brandDto, userInclusion);
            if (createdBrand == null)
            {
                return Conflict("A marca já existe.");
            }

            return CreatedAtAction(nameof(GetBrandById), new { id = createdBrand.Id }, createdBrand);
        }

        [HttpPut("MudaMarca/{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandDTO brandDto, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            if (id != brandDto.BrandId)
            {
                return BadRequest("ID da marca não corresponde.");
            }

            var updatedBrand = await _repository.UpdateBrandAsync(id, brandDto, userChange);
            if (updatedBrand == null)
            {
                return NotFound("Marca não encontrada.");
            }

            return Ok("Marca atualizada com sucesso.");
        }

        [HttpGet("VerMarcasPor/{id}")]
        public async Task<ActionResult<BrandDTO>> GetBrandById(int id)
        {
            var brand = await _repository.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound("Marca não encontrada ou não está inativa.");
            }

            var brandDto = new BrandDTO
            {
                BrandId = brand.Id,
                Name = brand.Name
            };

            return Ok(brandDto);
        }

        [HttpGet("VerTodosOsTiposDeProduto")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var brands = await _repository.GetAllInactiveBrandsAsync();
            if (brands == null || !brands.Any())
            {
                return NotFound("Nenhuma marca inativa encontrada.");
            }

            return Ok(brands);
        }

        [HttpDelete("DeletaMarca/{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var success = await _repository.DeleteBrandAsync(id);
            if (!success)
            {
                return NotFound("Marca não encontrada.");
            }

            return NoContent();
        }

        [HttpDelete("DesativarMarca/{id}")]
        public async Task<IActionResult> DisableBrand(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var success = await _repository.DisableBrandAsync(id, userChange);
            if (!success)
            {
                return NotFound($"Marca com ID {id} não encontrada.");
            }

            return NoContent();
        }
    }
}
