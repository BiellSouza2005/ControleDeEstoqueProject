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
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("VerPedidos")]
        public IActionResult GetAll()
        {
            var orders = _context.Orders
                .Where(o => !o.IsActive) // Filtra apenas os pedidos ativos
                .Include(op => op.OrderProducts)
                    .ThenInclude(pr => pr.Product)
                .Include(p => p.Payments)
                .ToList();

            return orders == null || !orders.Any() ? NotFound("Nenhum pedido ativo encontrado.") : Ok(orders);
        }

        [HttpGet("BuscarPedido/{id}")]
        public IActionResult GetById(int id)
        {
            var order = _context.Orders
                .Include(op => op.OrderProducts)
                    .ThenInclude(pr => pr.Product)
                .Include(p => p.Payments)
                .FirstOrDefault(o => o.Id == id && !o.IsActive); // Filtra pelo ID e IsActive

            return order == null ? NotFound($"Pedido com ID {id} não encontrado ou está inativo.") : Ok(order);
        }


        [HttpPost("AdicionarPedido")]
        public async Task<IActionResult> Create([FromBody] OrderDTO orderDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newOrder = new Order
            {
                OrderDate = orderDto.OrderDate,
                ClientId = orderDto.ClientId,
                Payments = new List<Payment>(),
                UserInclusion = userInclusion,
                UserChange = userInclusion
            };

            
            foreach (OrderProductDTO i in orderDto.OrderItems)
            {
                var orderProduct = new OrderProduct
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    UserInclusion = userInclusion,
                    UserChange = userInclusion

                };

                newOrder.OrderProducts.Add(orderProduct);
            }

            foreach (PaymentDTO i in orderDto.OrderPayments)
            {
                var orderPayment = new Payment
                {
                    Amount = i.Amount,
                    DueDate = i.PaymentDate,
                    PaymentStatus = PaymentStatus.Pendente,
                    UserInclusion = userInclusion,
                    UserChange = userInclusion

                };

                newOrder.Payments.Add(orderPayment);
            }

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, orderDto);
        }

        [HttpPut("AtualizarPedido/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDTO orderDto, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingOrder = await _context.Orders
                .Include(o => o.OrderProducts)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null)
            {
                return NotFound($"Pedido com ID {id} não encontrado.");
            }

            existingOrder.OrderDate = orderDto.OrderDate;
            existingOrder.ClientId = orderDto.ClientId;
            existingOrder.DateTimeChange = DateTime.UtcNow;
            existingOrder.UserChange = userChange;

            existingOrder.OrderProducts.Clear();
            foreach (var item in orderDto.OrderItems)
            {
                var orderProduct = new OrderProduct
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    UserInclusion = existingOrder.UserInclusion,
                    UserChange = userChange
                };

                existingOrder.OrderProducts.Add(orderProduct);
            }

            existingOrder.Payments.Clear();
            foreach (var payment in orderDto.OrderPayments)
            {
                var orderPayment = new Payment
                {
                    Amount = payment.Amount,
                    DueDate = payment.PaymentDate, // Assumindo que o PaymentDate do DTO é o DueDate
                    PaymentStatus = PaymentStatus.Pendente,
                    UserInclusion = existingOrder.UserInclusion,
                    UserChange = userChange
                };

                existingOrder.Payments.Add(orderPayment);
            }

            _context.Orders.Update(existingOrder);
            await _context.SaveChangesAsync();

            return Ok("Pedido atualizado com sucesso.");
        }

        [HttpDelete("DesativarPedido/{id}")]
        public async Task<IActionResult> Disable(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound($"Pedido com ID {id} não encontrado.");
            }

            order.IsActive = true;
            order.DateTimeChange = DateTime.UtcNow;
            order.UserChange = userChange;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
