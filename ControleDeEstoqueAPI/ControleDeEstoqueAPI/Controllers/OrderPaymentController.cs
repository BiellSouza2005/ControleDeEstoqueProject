using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.OrderPayment;
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
    public class OrderPaymentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderPaymentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("VerPagamentoDePedidos")]
        public IActionResult GetAll() =>
            Ok(_context.OrderPayments.ToList());

        [HttpGet("BuscarPagamentoDePedido/{id}")]
        public IActionResult GetById(int id)
        {
            var orderPayments = _context.OrderPayments.Find(id);
            return orderPayments == null ? NotFound() : Ok(orderPayments);
        }

        [HttpPost("AdicionarPagamentoDePedido")]
        public async Task<IActionResult> Create([FromBody] OrderPaymentDTO orderPaymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderPayments = new OrderPayment
            {
                OrderId = orderPaymentDto.OrderId,
                PaymentId = orderPaymentDto.PaymentId
            };

            _context.OrderPayments.Add(orderPayments);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = orderPayments.OrderPaymentId }, orderPayments);
        }

        [HttpPut("AlterarPagamentoDePedido/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderPaymentDTO orderPaymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderPayments = await _context.OrderPayments.FindAsync(id);
            if (orderPayments == null)
            {
                return NotFound();
            }

            orderPayments.OrderId = orderPaymentDto.OrderId;
            orderPayments.PaymentId = orderPaymentDto.PaymentId;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.OrderPayments.Any(e => e.OrderPaymentId == id))
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

        [HttpDelete("DeletarPagamentoDePedido/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderPayments = _context.OrderPayments.Find(id);
            if (orderPayments == null) return NotFound();

            _context.OrderPayments.Remove(orderPayments);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
