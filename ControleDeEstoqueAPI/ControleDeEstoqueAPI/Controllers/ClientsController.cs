using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Clients;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Data.DTOs.ProductType;
using ControleDeEstoqueAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() =>
            Ok(_context.Clients.ToList());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var client = _context.Clients.Find(id);
            return client == null ? NotFound() : Ok(client);
        }

        [HttpPost("AdicionarCliente")]
        public async Task<IActionResult> Create([FromBody] ClientDTO clientsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = new Client
            {
                Name = clientsDto.Name,
                Email = clientsDto.Email
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = client.ClientId }, client);
        }

        [HttpPut("AlterarCliente/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientDTO clientsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            client.Name = clientsDto.Name;
            client.Email = clientsDto.Email;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clients.Any(e => e.ClientId == id))
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

        [HttpDelete("DeletarCliente/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
