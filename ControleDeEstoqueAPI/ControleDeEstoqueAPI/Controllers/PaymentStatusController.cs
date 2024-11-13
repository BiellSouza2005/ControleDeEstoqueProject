using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.PaymentStatus;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsStatusController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentsStatusController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("VerStatusDePagamento")]
        public IActionResult GetAll() =>
            Ok(_context.PaymentStatuses.ToList());

        [HttpGet("BuscarStatusDePagamento/{id}")]
        public IActionResult GetById(int id)
        {
            var paymentStatus = _context.PaymentStatuses.Find(id);
            return paymentStatus == null ? NotFound() : Ok(paymentStatus);
        }

        [HttpPost("AdicionarStatusDePagamento")]
        public async Task<IActionResult> Create([FromBody] PaymentStatusDTO paymentStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentStatus = new PaymentStatus
            {
                Name = paymentStatusDto.Name
            };

            _context.PaymentStatuses.Add(paymentStatus);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = paymentStatus.PaymentStatusId }, paymentStatus);
        }

        [HttpPut("AlterarStatusDePagamento/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentStatusDTO paymentStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentStatus = await _context.PaymentStatuses.FindAsync(id);
            if (paymentStatus == null)
            {
                return NotFound();
            }

            paymentStatus.Name = paymentStatusDto.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PaymentStatuses.Any(e => e.PaymentStatusId == id))
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

        [HttpDelete("DeletarStatusDePagamento/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var paymentStatus = _context.PaymentStatuses.Find(id);
            if (paymentStatus == null) return NotFound();

            _context.PaymentStatuses.Remove(paymentStatus);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
