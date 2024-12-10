using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("VerProdutoPor/{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
                return NotFound("Produto não encontrado ou não está inativo.");

            return Ok(product);
        }

        [HttpGet("VerTodosProdutos")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllInactiveProductsAsync();

            if (products == null || !products.Any())
                return NotFound("Nenhum produto inativo encontrado.");

            return Ok(products);
        }

        [HttpPost("AdicionarProduto")]
        public async Task<IActionResult> Create([FromBody] ProductDTO productDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepository.AddProductAsync(productDto, userInclusion);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("AlterarProduto/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductWhithoutQntDTO productDto, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            if (id != productDto.ProductId)
                return BadRequest("ID do produto não corresponde.");

            var product = await _productRepository.UpdateProductAsync(id, productDto, userChange);

            if (product == null)
                return NotFound("Produto não encontrado.");

            return Ok("Produto atualizado com sucesso.");
        }

        [HttpPut("AdicionarQuantidade/{id}/{quantidade}")]
        public async Task<IActionResult> AddQuantity(int id, int quantidade, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var product = await _productRepository.AddQuantityAsync(id, quantidade, userChange);

            if (product == null)
                return NotFound("Produto não encontrado.");

            return Ok(product);
        }

        [HttpPut("SubtrairQuantidade/{id}/{quantidade}")]
        public async Task<IActionResult> SubtractQuantity(int id, int quantidade, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var product = await _productRepository.SubtractQuantityAsync(id, quantidade, userChange);

            if (product == null)
                return BadRequest("Quantidade insuficiente no estoque ou produto não encontrado.");

            return Ok(product);
        }

        [HttpDelete("DeletarProduto/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productRepository.DeleteProductAsync(id);

            if (!deleted)
                return NotFound("Produto não encontrado.");

            return NoContent();
        }

        [HttpDelete("DesativarProduto/{id}")]
        public async Task<IActionResult> Disable(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var disabled = await _productRepository.DisableProductAsync(id, userChange);

            if (!disabled)
                return NotFound("Produto não encontrado.");

            return NoContent();
        }
    }
}
