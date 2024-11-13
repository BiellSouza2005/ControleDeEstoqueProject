using ControleDeEstoqueAPI.Entities;
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
        public DbSet<OrderPayment> OrderPayments { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderPayment>()
                .HasKey(op => new { op.OrderId, op.PaymentId });

            modelBuilder.Entity<ProductDescription>()
                .HasKey(pd => pd.ProductId);

            // Definindo a precisão dos campos decimais, 18 dígitos no total, 2 casas decimais
            modelBuilder.Entity<OrderProduct>()
                .Property(op => op.UnitPrice)
                .HasPrecision(18, 2); 

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PaymentHistory>()
                .Property(ph => ph.NewAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PaymentHistory>()
                .Property(ph => ph.PreviousAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
