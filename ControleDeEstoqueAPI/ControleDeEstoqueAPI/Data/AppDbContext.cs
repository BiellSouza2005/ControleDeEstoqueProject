using ControleDeEstoqueAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoqueAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductDescription> ProductDescriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<OrderPayment> OrderPayments { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderPayment>()
                .HasKey(op => new { op.OrderId, op.PaymentId });

            modelBuilder.Entity<ProductDescription>()
                .HasKey(pd => pd.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
