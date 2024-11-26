using ControleDeEstoqueAPI.Data.DTOs.ProductDescription;
using ControleDeEstoqueAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Repositories
{
    public interface IProductDescriptionRepository
    {
        Task<ProductDescriptionDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDescriptionDTO>> GetAllInactiveAsync();
        Task AddAsync(ProductDescriptionDTO productDescriptionDto, string userInclusion);
        Task UpdateAsync(ProductDescriptionDTO productDescriptionDto, string userChange);
        Task RemoveAsync(int id);
        Task DisableAsync(int id, string userChange);
    }
}
