using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.ProductDescription;
using ControleDeEstoqueAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeEstoqueAPI.Repositories
{
    public class ProductDescriptionRepository : IProductDescriptionRepository
    {
        private readonly AppDbContext _context;

        public ProductDescriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDescriptionDTO?> GetByIdAsync(int id)
        {
            return await _context.ProductDescriptions
                .AsNoTracking()
                .Where(pd => pd.Id == id && !pd.IsActive)
                .Select(pd => new ProductDescriptionDTO
                {
                    ProductDescriptionId = pd.Id,
                    ProductId = pd.ProductId,
                    Description = pd.Description
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDescriptionDTO>> GetAllInactiveAsync()
        {
            return await _context.ProductDescriptions
                .AsNoTracking()
                .Where(pd => !pd.IsActive)
                .Select(pd => new ProductDescriptionDTO
                {
                    ProductDescriptionId = pd.Id,
                    ProductId = pd.ProductId,
                    Description = pd.Description
                })
                .ToListAsync();
        }

        public async Task AddAsync(ProductDescriptionDTO productDescriptionDto, string userInclusion)
        {
            var productDescription = new ProductDescription
            {
                ProductId = productDescriptionDto.ProductId,
                Description = productDescriptionDto.Description,
                UserInclusion = userInclusion,
                UserChange = userInclusion
            };

            await _context.ProductDescriptions.AddAsync(productDescription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductDescriptionDTO productDescriptionDto, string userChange)
        {
            var productDescription = await _context.ProductDescriptions.FindAsync(productDescriptionDto.ProductDescriptionId);

            if (productDescription == null)
                throw new KeyNotFoundException("Descrição do produto não encontrada.");

            productDescription.ProductId = productDescriptionDto.ProductId;
            productDescription.Description = productDescriptionDto.Description;
            productDescription.DateTimeChange = DateTime.UtcNow;
            productDescription.UserChange = userChange;

            _context.ProductDescriptions.Update(productDescription);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var productDescription = await _context.ProductDescriptions.FindAsync(id);

            if (productDescription == null)
                throw new KeyNotFoundException("Descrição do produto não encontrada.");

            _context.ProductDescriptions.Remove(productDescription);
            await _context.SaveChangesAsync();
        }

        public async Task DisableAsync(int id, string userChange)
        {
            var productDescription = await _context.ProductDescriptions.FindAsync(id);

            if (productDescription == null)
                throw new KeyNotFoundException("Descrição do produto não encontrada.");

            productDescription.IsActive = true;
            productDescription.DateTimeChange = DateTime.UtcNow;
            productDescription.UserChange = userChange;

            _context.ProductDescriptions.Update(productDescription);
            await _context.SaveChangesAsync();
        }
    }
}
