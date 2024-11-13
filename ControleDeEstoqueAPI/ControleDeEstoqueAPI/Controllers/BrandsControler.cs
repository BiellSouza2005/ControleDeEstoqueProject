using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Entities;
using ControleDeEstoqueAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoqueAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/VerMarcas")]
        public IActionResult GetAll() =>
            Ok(_context.Brands.ToList());

        [HttpGet("BuscaMarca/{id}")]
        public IActionResult GetById(int id)
        {
            var brand = _context.Brands.Find(id);
            return brand == null ? NotFound() : Ok(brand);
        }

        [HttpPost("AdicionaMarca")]
        public async Task<IActionResult> Create([FromBody] BrandDTO brandDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brand = new Brand
            {
                Name = brandDto.Name
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = brand.BrandId }, brand);
        }

        [HttpPut("MudaMarca/{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandDTO brandDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            brand.Name = brandDTO.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Brands.Any(e => e.BrandId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("DeletaMarca/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null) return NotFound();

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
