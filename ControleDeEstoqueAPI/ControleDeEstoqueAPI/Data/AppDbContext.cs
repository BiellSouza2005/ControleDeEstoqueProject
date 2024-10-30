using ControleDeEstoqueAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoqueAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Mark> Marks { get; set; }
    }
}
