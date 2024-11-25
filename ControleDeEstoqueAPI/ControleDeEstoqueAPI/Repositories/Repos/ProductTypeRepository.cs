using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.ProductType;
using ControleDeEstoqueAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Repositories
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly AppDbContext _context;

        public ProductTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductType> AddProductTypeAsync(ProductTypeDTO productTypeDto, string userInclusion)
        {
            var existingProductType = await _context.ProductTypes
                .FirstOrDefaultAsync(pt => pt.Name == productTypeDto.Name);
            if (existingProductType != null) return null;

            var productType = new ProductType
            {
                Name = productTypeDto.Name,
                UserInclusion = userInclusion,
                UserChange = userInclusion
            };

            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
            return productType;
        }

        public async Task<ProductType> UpdateProductTypeAsync(int id, ProductTypeDTO productTypeDto, string userChange)
        {
            var existingProductType = await _context.ProductTypes.FindAsync(id);
            if (existingProductType == null) return null;

            existingProductType.Name = productTypeDto.Name;
            existingProductType.DateTimeChange = DateTime.UtcNow;
            existingProductType.UserChange = userChange;

            _context.ProductTypes.Update(existingProductType);
            await _context.SaveChangesAsync();
            return existingProductType;
        }

        public async Task<ProductType> GetProductTypeByIdAsync(int id)
        {
            return await _context.ProductTypes
                .FirstOrDefaultAsync(pt => pt.Id == id && !pt.IsActive);
        }

        public async Task<IEnumerable<ProductTypeDTO>> GetAllInactiveProductTypesAsync()
        {
            return await _context.ProductTypes
                .Where(pt => !pt.IsActive)
                .Select(pt => new ProductTypeDTO
                {
                    ProductTypeId = pt.Id,
                    Name = pt.Name
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteProductTypeAsync(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null) return false;

            _context.ProductTypes.Remove(productType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DisableProductTypeAsync(int id, string userChange)
        {
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null) return false;

            productType.IsActive = true;
            productType.DateTimeChange = DateTime.UtcNow;
            productType.UserChange = userChange;

            _context.ProductTypes.Update(productType);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
