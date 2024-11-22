using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Product;
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
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("VerProdutoPor/{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id && !p.IsActive) // Filtra usuários com IsActive = false
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound("Produto não encontrado ou não está inativo.");
            }

            var productDto = new ProductDTO
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                BrandId = product.BrandId,
                ProductTypeId = product.ProductTypeId 
            };

            return Ok(productDto);
        }

        [HttpGet("VerTodosProdutos")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var products = await _context.Products
                .Where(p => !p.IsActive) // Filtra apenas usuários com IsActive = false
                .Select(p => new ProductDTO
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    BrandId = p.BrandId,
                    ProductTypeId = p.ProductTypeId
                })
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound("Nenhum produto inativo encontrado.");
            }

            return Ok(products);
        }

        [HttpPost("AdicionarProduto")]
        public async Task<IActionResult> Create([FromBody] ProductDTO productDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
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
                ProductTypeId = productDto.ProductTypeId,
                UserInclusion = userInclusion,
                UserChange = userInclusion,
                
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("AlterarProduto/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDTO productDto, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            if (id != productDto.ProductId)
            {
                return BadRequest("ID do produto não corresponde.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound("Produto não encontrado.");
            }

            existingProduct.Name = productDto.Name;
            existingProduct.Price = productDto.Price;
            existingProduct.BrandId = productDto.BrandId;
            existingProduct.DateTimeChange = DateTime.UtcNow;
            existingProduct.UserChange = userChange;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return Ok("Produto atualizado com sucesso.");
        }

        [HttpPut("AdicionarQuantidade/{id}/{quantidade}")]
        public async Task<IActionResult> AddQuantity(int id, int quantidade, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound("Produto não encontrado.");

            product.Quantity += quantidade;
            product.IsActive = true;
            product.DateTimeChange = DateTime.UtcNow;
            product.UserChange = userChange;
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut("SubtrairQuantidade/{id}/{quantidade}")]
        public async Task<IActionResult> SubtractQuantity(int id, int quantidade, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound("Produto não encontrado.");

            if (product.Quantity < quantidade)
            {
                return BadRequest("Quantidade insuficiente no estoque.");
            }

            product.Quantity -= quantidade;
            product.IsActive = true;
            product.DateTimeChange = DateTime.UtcNow;
            product.UserChange = userChange;
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

        [HttpDelete("DesativarProduto/{id}")]
        public async Task<IActionResult> Disable(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound($"Usuário com ID {id} não encontrado.");
            }

            product.IsActive = true;
            product.DateTimeChange = DateTime.UtcNow;
            product.UserChange = userChange;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
