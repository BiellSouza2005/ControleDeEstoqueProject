using ControleDeEstoqueAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoqueAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) // Passa as opções para a classe base DbContext
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductDescription> ProductDescriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{ 
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
