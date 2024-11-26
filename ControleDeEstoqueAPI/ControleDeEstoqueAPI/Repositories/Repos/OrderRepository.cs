using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Order;
using ControleDeEstoqueAPI.Data.DTOs.OrderProduct;
using ControleDeEstoqueAPI.Data.DTOs.Payment;
using ControleDeEstoqueAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            return await _context.Orders
                .Where(o => !o.IsActive)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.Payments)
                .Select(o => new OrderDTO
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    ClientId = o.ClientId,
                    OrderItems = o.OrderProducts.Select(op => new OrderProductDTO
                    {
                        ProductId = op.ProductId,
                        Quantity = op.Quantity,
                        UnitPrice = op.UnitPrice
                    }).ToList(),
                    OrderPayments = o.Payments.Select(p => new PaymentDTO
                    {
                        Amount = p.Amount,
                        PaymentDate = p.DueDate
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<OrderDTO?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Where(o => o.Id == id && !o.IsActive)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.Payments)
                .Select(o => new OrderDTO
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    ClientId = o.ClientId,
                    OrderItems = o.OrderProducts.Select(op => new OrderProductDTO
                    {
                        ProductId = op.ProductId,
                        Quantity = op.Quantity,
                        UnitPrice = op.UnitPrice
                    }).ToList(),
                    OrderPayments = o.Payments.Select(p => new PaymentDTO
                    {
                        Amount = p.Amount,
                        PaymentDate = p.DueDate
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task AddAsync(OrderDTO orderDto, string userInclusion)
        {
            var newOrder = new Order
            {
                OrderDate = orderDto.OrderDate,
                ClientId = orderDto.ClientId,
                UserInclusion = userInclusion,
                UserChange = userInclusion
            };

            foreach (var item in orderDto.OrderItems)
            {
                newOrder.OrderProducts.Add(new OrderProduct
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    UserInclusion = userInclusion,
                    UserChange = userInclusion
                });
            }

            foreach (var payment in orderDto.OrderPayments)
            {
                newOrder.Payments.Add(new Payment
                {
                    Amount = payment.Amount,
                    DueDate = payment.PaymentDate,
                    PaymentStatus = PaymentStatus.Pendente,
                    UserInclusion = userInclusion,
                    UserChange = userInclusion
                });
            }

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, OrderDTO orderDto, string userChange)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.OrderProducts)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null)
                throw new KeyNotFoundException("Pedido não encontrado.");

            existingOrder.OrderDate = orderDto.OrderDate;
            existingOrder.ClientId = orderDto.ClientId;
            existingOrder.DateTimeChange = DateTime.UtcNow;
            existingOrder.UserChange = userChange;

            existingOrder.OrderProducts.Clear();
            foreach (var item in orderDto.OrderItems)
            {
                existingOrder.OrderProducts.Add(new OrderProduct
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    UserInclusion = existingOrder.UserInclusion,
                    UserChange = userChange
                });
            }

            existingOrder.Payments.Clear();
            foreach (var payment in orderDto.OrderPayments)
            {
                existingOrder.Payments.Add(new Payment
                {
                    Amount = payment.Amount,
                    DueDate = payment.PaymentDate,
                    PaymentStatus = PaymentStatus.Pendente,
                    UserInclusion = existingOrder.UserInclusion,
                    UserChange = userChange
                });
            }

            _context.Orders.Update(existingOrder);
            await _context.SaveChangesAsync();
        }

        public async Task DisableAsync(int id, string userChange)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                throw new KeyNotFoundException("Pedido não encontrado.");

            order.IsActive = true;
            order.DateTimeChange = DateTime.UtcNow;
            order.UserChange = userChange;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
