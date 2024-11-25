using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Clients;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Data.DTOs.ProductType;
using ControleDeEstoqueAPI.Entities;
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

        [HttpGet("VerClientesPor/{id}")]
        public async Task<ActionResult<ClientDTO>> GetClientsById(int id)
        {
            var client = await _context.Clients
                .Where(c => c.Id == id && !c.IsActive) // Filtra usuários com IsActive = false
                .FirstOrDefaultAsync();

            if (client == null)
            {
                return NotFound("Cliente não encontrado ou não está inativa.");
            }

            var clientsDto = new ClientDTO
            {
                ClientId = client.Id,
                Name = client.Name,
                Email = client.Email
            };

            return Ok(clientsDto);
        }

        [HttpGet("VerTodosOsClientes")]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetAllBrands()
        {
            var client = await _context.Clients
                .Where(c => !c.IsActive) // Filtra apenas usuários com IsActive = false
                .Select(c => new ClientDTO
                {
                    ClientId = c.Id,
                    Name = c.Name,
                    Email = c.Email
                })
                .ToListAsync();

            if (!client.Any())
            {
                return NotFound("Nenhum cliente inativo encontrado.");
            }

            return Ok(client);
        }

        [HttpPost("AdicionarCliente")]
        public async Task<IActionResult> Create([FromBody] ClientDTO clientsDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = new Client
            {
                Name = clientsDto.Name,
                Email = clientsDto.Email,
                UserInclusion = userInclusion,
                UserChange = userInclusion
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClientsById), new { id = client.Id }, client);
        }

        [HttpPut("AlterarCliente/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientDTO clientsDto, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            if (id != clientsDto.ClientId)
            {
                return BadRequest("ID do CLiente não corresponde.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCLient = await _context.Clients.FindAsync(id);

            if (existingCLient == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            existingCLient.Name = clientsDto.Name;
            existingCLient.Email = clientsDto.Email;
            existingCLient.DateTimeChange = DateTime.UtcNow;
            existingCLient.UserChange = userChange;

            _context.Clients.Update(existingCLient);
            await _context.SaveChangesAsync();

            return Ok("Cliente atualizado com sucesso.");
        }

        [HttpDelete("DeletarCliente/{id}")]
        public async Task<IActionResult> Delete(int id, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("DesativarMarca/{id}")]
        public async Task<IActionResult> Disable(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound($"Marca com ID {id} não encontrado.");
            }

            client.IsActive = true;
            client.DateTimeChange = DateTime.UtcNow;
            client.UserChange = userChange;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
