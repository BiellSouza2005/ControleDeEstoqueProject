using ControleDeEstoqueAPI.Data.DTOs.ProductDescription;
using ControleDeEstoqueAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDescriptionController : ControllerBase
    {
        private readonly IProductDescriptionRepository _repository;

        public ProductDescriptionController(IProductDescriptionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDescriptionDTO>> GetProductDescriptionById(int id)
        {
            var productDescription = await _repository.GetByIdAsync(id);

            if (productDescription == null)
                return NotFound("Descrição do produto não encontrada ou já está ativa.");

            return Ok(productDescription);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDescriptionDTO>>> GetAllProductDescriptions()
        {
            var productDescriptions = await _repository.GetAllInactiveAsync();

            if (!productDescriptions.Any())
                return NotFound("Nenhuma descrição de produto inativa encontrada.");

            return Ok(productDescriptions);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDescriptionDTO productDescriptionDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.AddAsync(productDescriptionDto, userInclusion);

            return CreatedAtAction(nameof(GetProductDescriptionById), new { id = productDescriptionDto.ProductDescriptionId }, productDescriptionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDescriptionDTO productDescriptionDto, [FromHeader(Name = "User-Change")] string userChange)
        {
            if (id != productDescriptionDto.ProductDescriptionId)
                return BadRequest("ID da descrição do produto não corresponde.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.UpdateAsync(productDescriptionDto, userChange);

            return Ok("Descrição do produto atualizada com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.RemoveAsync(id);
            return NoContent();
        }

        [HttpPut("disable/{id}")]
        public async Task<IActionResult> Disable(int id, [FromHeader(Name = "User-Change")] string userChange)
        {
            await _repository.DisableAsync(id, userChange);
            return NoContent();
        }
    }
}
