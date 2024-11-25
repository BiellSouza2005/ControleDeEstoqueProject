using ControleDeEstoqueAPI.Data.DTOs.Clients;
using ControleDeEstoqueAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _repository;

        public ClientsController(IClientRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("AdicionarCliente")]
        public async Task<IActionResult> Create([FromBody] ClientDTO clientDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdClient = await _repository.AddClientAsync(clientDto, userInclusion);
            return CreatedAtAction(nameof(GetClientsById), new { id = createdClient.Id }, createdClient);
        }

        [HttpPut("AlterarCliente/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientDTO clientDto, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            if (id != clientDto.ClientId)
            {
                return BadRequest("ID do Cliente não corresponde.");
            }

            var updatedClient = await _repository.UpdateClientAsync(id, clientDto, userChange);
            if (updatedClient == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok("Cliente atualizado com sucesso.");
        }

        [HttpGet("VerClientesPor/{id}")]
        public async Task<ActionResult<ClientDTO>> GetClientsById(int id)
        {
            var client = await _repository.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound("Cliente não encontrado ou não está inativo.");
            }

            var clientDto = new ClientDTO
            {
                ClientId = client.Id,
                Name = client.Name,
                Email = client.Email
            };

            return Ok(clientDto);
        }

        [HttpGet("VerTodosOsClientes")]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetAllClients()
        {
            var clients = await _repository.GetAllInactiveClientsAsync();
            if (clients == null)
            {
                return NotFound("Nenhum cliente inativo encontrado.");
            }

            return Ok(clients);
        }

        [HttpDelete("DeletarCliente/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repository.DeleteClientAsync(id);
            if (!success)
            {
                return NotFound("Cliente não encontrado.");
            }

            return NoContent();
        }

        [HttpDelete("DesativarCliente/{id}")]
        public async Task<IActionResult> Disable(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var success = await _repository.DisableClientAsync(id, userChange);
            if (!success)
            {
                return NotFound($"Cliente com ID {id} não encontrado.");
            }

            return NoContent();
        }
    }
}
