using ControleDeEstoqueAPI.Data.DTOs.ProductType;
using ControleDeEstoqueAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductTypesController : ControllerBase
    {
        private readonly IProductTypeRepository _productTypeRepository;

        public ProductTypesController(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }

        [HttpGet("VerTiposDeProdutosPor/{id}")]
        public async Task<ActionResult<ProductTypeDTO>> GetProductTypeById(int id)
        {
            var productType = await _productTypeRepository.GetProductTypeByIdAsync(id);

            if (productType == null)
            {
                return NotFound("Tipo de Produto não encontrado ou não está inativo.");
            }

            return Ok(productType);
        }

        [HttpGet("VerTodosOsTiposDeProduto")]
        public async Task<ActionResult<IEnumerable<ProductTypeDTO>>> GetAllProductTypes()
        {
            var productTypes = await _productTypeRepository.GetAllInactiveProductTypesAsync();

            if (productTypes == null)
            {
                return NotFound("Nenhum tipo de produto inativo encontrado.");
            }

            return Ok(productTypes);
        }

        [HttpPost("AdicionarTipoDeProduto")]
        public async Task<IActionResult> Create([FromBody] ProductTypeDTO productTypeDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productType = await _productTypeRepository.AddProductTypeAsync(productTypeDto, userInclusion);

            if (productType == null)
            {
                return Conflict("Tipo de Produto já existente.");
            }

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

            var updatedProductType = await _productTypeRepository.UpdateProductTypeAsync(id, productTypeDto, userChange);

            if (updatedProductType == null)
            {
                return NotFound("Tipo de Produto não encontrado.");
            }

            return Ok("Tipo de Produto atualizado com sucesso.");
        }

        [HttpDelete("DeletarTipoDeProduto/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productTypeRepository.DeleteProductTypeAsync(id);

            if (!deleted)
            {
                return NotFound("Tipo de Produto não encontrado.");
            }

            return NoContent();
        }

        [HttpDelete("DesativarTipoDeProduto/{id}")]
        public async Task<IActionResult> Disable(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var disabled = await _productTypeRepository.DisableProductTypeAsync(id, userChange);

            if (!disabled)
            {
                return NotFound($"Tipo de Produto com ID {id} não encontrado.");
            }

            return NoContent();
        }
    }
}
