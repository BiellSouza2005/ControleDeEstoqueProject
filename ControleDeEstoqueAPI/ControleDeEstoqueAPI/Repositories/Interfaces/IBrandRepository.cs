using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBrandRepository
{
    Task<Brand> AddBrandAsync(BrandDTO brandDto, string userInclusion);
    Task<Brand> UpdateBrandAsync(int id, BrandDTO brandDto, string userChange);
    Task<Brand> GetBrandByIdAsync(int id);
    Task<IEnumerable<BrandDTO>> GetAllInactiveBrandsAsync();
    Task<bool> DeleteBrandAsync(int id);
    Task<bool> DisableBrandAsync(int id, string userChange);
}
