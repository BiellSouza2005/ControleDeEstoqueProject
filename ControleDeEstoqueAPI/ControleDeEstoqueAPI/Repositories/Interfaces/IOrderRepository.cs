using ControleDeEstoqueAPI.Data.DTOs.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDTO>> GetAllAsync();
        Task<OrderDTO?> GetByIdAsync(int id);
        Task AddAsync(OrderDTO orderDto, string userInclusion);
        Task UpdateAsync(int id, OrderDTO orderDto, string userChange);
        Task DisableAsync(int id, string userChange);
    }
}
