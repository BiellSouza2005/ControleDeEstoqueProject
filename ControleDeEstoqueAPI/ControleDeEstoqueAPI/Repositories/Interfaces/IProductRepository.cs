using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Entities;

namespace ControleDeEstoqueAPI.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetAllInactiveProductsAsync();
        Task<Product> AddProductAsync(ProductDTO productDto, string userInclusion);
        Task<Product> UpdateProductAsync(int id, ProductWhithoutQntDTO productDto, string userChange);
        Task<Product> AddQuantityAsync(int id, int quantity, string userChange);
        Task<Product> SubtractQuantityAsync(int id, int quantity, string userChange);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> DisableProductAsync(int id, string userChange);
    }
}
