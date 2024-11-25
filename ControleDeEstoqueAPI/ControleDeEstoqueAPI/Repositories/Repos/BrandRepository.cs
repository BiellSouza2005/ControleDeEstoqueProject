using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BrandRepository : IBrandRepository
{
    private readonly AppDbContext _context;

    public BrandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Brand> AddBrandAsync(BrandDTO brandDto, string userInclusion)
    {
        var existingBrand = await _context.Brands
            .FirstOrDefaultAsync(b => b.Name == brandDto.Name);
        if (existingBrand != null) return null;

        var brand = new Brand
        {
            Name = brandDto.Name,
            UserInclusion = userInclusion,
            UserChange = userInclusion
        };

        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();
        return brand;
    }

    public async Task<Brand> UpdateBrandAsync(int id, BrandDTO brandDto, string userChange)
    {
        var existingBrand = await _context.Brands.FindAsync(id);
        if (existingBrand == null) return null;

        existingBrand.Name = brandDto.Name;
        existingBrand.DateTimeChange = DateTime.UtcNow;
        existingBrand.UserChange = userChange;

        _context.Brands.Update(existingBrand);
        await _context.SaveChangesAsync();
        return existingBrand;
    }

    public async Task<Brand> GetBrandByIdAsync(int id)
    {
        return await _context.Brands
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsActive);
    }

    public async Task<IEnumerable<BrandDTO>> GetAllInactiveBrandsAsync()
    {
        return await _context.Brands
            .Where(b => !b.IsActive)
            .Select(b => new BrandDTO
            {
                BrandId = b.Id,
                Name = b.Name
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteBrandAsync(int id)
    {
        var brand = await _context.Brands.FindAsync(id);
        if (brand == null) return false;

        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DisableBrandAsync(int id, string userChange)
    {
        var brand = await _context.Brands.FindAsync(id);
        if (brand == null) return false;

        brand.IsActive = true;
        brand.DateTimeChange = DateTime.UtcNow;
        brand.UserChange = userChange;

        _context.Brands.Update(brand);
        await _context.SaveChangesAsync();
        return true;
    }
}
