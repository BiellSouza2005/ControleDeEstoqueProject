using ControleDeEstoqueAPI.Data.DTOs.Order;
using ControleDeEstoqueAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;

        public OrderController(IOrderRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("VerPedidos")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _repository.GetAllAsync();
            return orders == null ? NotFound("Nenhum pedido ativo encontrado.") : Ok(orders);
        }

        [HttpGet("BuscarPedido/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            return order == null ? NotFound($"Pedido com ID {id} não encontrado ou está inativo.") : Ok(order);
        }

        [HttpPost("AdicionarPedido")]
        public async Task<IActionResult> Create([FromBody] OrderDTO orderDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.AddAsync(orderDto, userInclusion);
            return CreatedAtAction(nameof(GetById), new { id = orderDto.OrderId }, orderDto);
        }

        [HttpPut("AtualizarPedido/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDTO orderDto, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.UpdateAsync(id, orderDto, userChange);
            return Ok("Pedido atualizado com sucesso.");
        }

        [HttpDelete("DesativarPedido/{id}")]
        public async Task<IActionResult> Disable(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            await _repository.DisableAsync(id, userChange);
            return NoContent();
        }
    }
}
