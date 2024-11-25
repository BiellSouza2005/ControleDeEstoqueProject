using ControleDeEstoqueAPI.Data.DTOs.ProductType;
using ControleDeEstoqueAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Repositories
{
    public interface IProductTypeRepository
    {
        Task<ProductType> AddProductTypeAsync(ProductTypeDTO productTypeDto, string userInclusion);
        Task<ProductType> UpdateProductTypeAsync(int id, ProductTypeDTO productTypeDto, string userChange);
        Task<ProductType> GetProductTypeByIdAsync(int id);
        Task<IEnumerable<ProductTypeDTO>> GetAllInactiveProductTypesAsync();
        Task<bool> DeleteProductTypeAsync(int id);
        Task<bool> DisableProductTypeAsync(int id, string userChange);
    }
}
