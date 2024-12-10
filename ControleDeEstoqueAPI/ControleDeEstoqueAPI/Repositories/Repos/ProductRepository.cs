using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Where(p => p.Id == id && !p.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDTO>> GetAllInactiveProductsAsync()
        {
            return await _context.Products
                .Where(p => !p.IsActive)
                .Select(p => new ProductDTO
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    BrandId = p.BrandId,
                    ProductTypeId = p.ProductTypeId
                })
                .ToListAsync();
        }

        public async Task<Product> AddProductAsync(ProductDTO productDto, string userInclusion)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                BrandId = productDto.BrandId,
                ProductTypeId = productDto.ProductTypeId,
                UserInclusion = userInclusion,
                UserChange = userInclusion
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, ProductWhithoutQntDTO productDto, string userChange)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return null;

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.BrandId = productDto.BrandId;
            product.DateTimeChange = DateTime.UtcNow;
            product.UserChange = userChange;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> AddQuantityAsync(int id, int quantity, string userChange)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return null;

            product.Quantity += quantity;
            product.DateTimeChange = DateTime.UtcNow;
            product.UserChange = userChange;

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> SubtractQuantityAsync(int id, int quantity, string userChange)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null || product.Quantity < quantity)
                return null;

            product.Quantity -= quantity;
            product.DateTimeChange = DateTime.UtcNow;
            product.UserChange = userChange;

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DisableProductAsync(int id, string userChange)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return false;

            product.IsActive = true;
            product.DateTimeChange = DateTime.UtcNow;
            product.UserChange = userChange;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
