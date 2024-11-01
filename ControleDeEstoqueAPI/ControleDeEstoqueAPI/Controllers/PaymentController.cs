using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Payment;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Data.DTOs.ProductDescription;
using ControleDeEstoqueAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("VerPagamentos")]
        public IActionResult GetAll() =>
            Ok(_context.Payments.ToList());

        [HttpGet("BuscarPagamentos/{id}")]
        public IActionResult GetById(int id)
        {
            var payments = _context.Payments.Find(id);
            return payments == null ? NotFound() : Ok(payments);
        }

        [HttpPost("AdicionarPagamento")]
        public async Task<IActionResult> Create([FromBody] PaymentDTO paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payments = new Payment
            {
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate
            };

            _context.Payments.Add(payments);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = payments.PaymentId }, payments);
        }

        [HttpPut("AlterarPagamento/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentDTO paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payments = await _context.Payments.FindAsync(id);
            if (payments == null)
            {
                return NotFound();
            }

            payments.Amount = paymentDto.Amount;
            payments.PaymentDate = paymentDto.PaymentDate;
        

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Payments.Any(e => e.PaymentId == id))
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

        [HttpDelete("DeletarPagamento/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = _context.Payments.Find(id);
            if (payment == null) return NotFound();

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
