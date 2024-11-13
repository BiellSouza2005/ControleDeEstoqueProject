using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Order;
using ControleDeEstoqueAPI.Data.DTOs.OrderProduct;
using ControleDeEstoqueAPI.Data.DTOs.Payment;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Data.DTOs.ProductDescription;
using ControleDeEstoqueAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("VerProdutosDePedido")]
        public IActionResult GetAll() =>
            Ok(_context.OrderProducts.ToList());

        [HttpGet("BuscarProdutoDoPedido/{id}")]
        public IActionResult GetById(int id)
        {
            var orderProducts = _context.OrderProducts.Find(id);
            return orderProducts == null ? NotFound() : Ok(orderProducts);
        }

        [HttpPost("AdicionarProdutoNoPedido")]
        public async Task<IActionResult> Create([FromBody] OrderProductDTO orderProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderProducts = new OrderProduct
            {
                OrderId = orderProductDto.OrderId,
                ProductId = orderProductDto.ProductId,
                Quantity = orderProductDto.Quantity,
                UnitPrice = orderProductDto.UnitPrice
            };

            _context.OrderProducts.Add(orderProducts);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = orderProducts.OrderId }, orderProducts);
        }

        [HttpPut("AlterarProdutoNoPedido/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderProductDTO orderProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderProducts = await _context.OrderProducts.FindAsync(id);
            if (orderProducts == null)
            {
                return NotFound();
            }

            orderProducts.OrderId = orderProductDto.OrderId;
            orderProducts.ProductId = orderProductDto.ProductId;
            orderProducts.Quantity = orderProductDto.Quantity;
            orderProducts.UnitPrice = orderProductDto.UnitPrice;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.OrderProducts.Any(e => e.OrderProductId == id))
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

        [HttpDelete("DeletarProdutoNoPedido/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderProducts = _context.OrderProducts.Find(id);
            if (orderProducts == null) return NotFound();

            _context.OrderProducts.Remove(orderProducts);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
