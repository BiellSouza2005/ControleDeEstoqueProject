using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Order;
using ControleDeEstoqueAPI.Data.DTOs.Payment;
using ControleDeEstoqueAPI.Data.DTOs.PaymentHistory;
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
    public class PaymentHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentHistoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("VerHistoricoDePagamentos")]
        public IActionResult GetAll() =>
            Ok(_context.PaymentHistories.ToList());

        [HttpGet("BuscarHistoricoDePagamento/{id}")]
        public IActionResult GetById(int id)
        {
            var paymentHistories = _context.PaymentHistories.Find(id);
            return paymentHistories == null ? NotFound() : Ok(paymentHistories);
        }

        [HttpPost("AdicionarHistoricoDePagamento")]
        public async Task<IActionResult> Create([FromBody] PaymentHistoryDTO paymentHistoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentHistories = new PaymentHistory
            {
                PaymentId = paymentHistoryDto.PaymentId,
                PreviousAmount = paymentHistoryDto.PreviousAmount,
                NewAmount = paymentHistoryDto.NewAmount,
                ModificationDate = paymentHistoryDto.ModificationDate
            };

            _context.PaymentHistories.Add(paymentHistories);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = paymentHistories.PaymentHistoryId }, paymentHistories);
        }

        [HttpPut("AlterarHistoricoDePagamento/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentHistoryDTO paymentHistoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentHistories = await _context.PaymentHistories.FindAsync(id);
            if (paymentHistories == null)
            {
                return NotFound();
            }

            paymentHistories.PaymentId = paymentHistoryDto.PaymentId;
            paymentHistories.PreviousAmount = paymentHistoryDto.PreviousAmount;
            paymentHistories.NewAmount = paymentHistoryDto.NewAmount;
            paymentHistories.ModificationDate = paymentHistoryDto.ModificationDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PaymentHistories.Any(e => e.PaymentHistoryId == id))
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

        [HttpDelete("DeletarHistoricoDePagamento/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var paymentHistories = _context.PaymentHistories.Find(id);
            if (paymentHistories == null) return NotFound();

            _context.PaymentHistories.Remove(paymentHistories);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
