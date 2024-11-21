using ControleDeEstoqueAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeEstoqueAPI.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Property(c => c.DateTimeInclusion)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(c => c.UserInclusion)
                   .IsRequired()
                   .HasMaxLength(80);

            builder.Property(c => c.DateTimeChange)
                   .IsRequired();

            builder.Property(c => c.UserChange)
                   .HasMaxLength(80);

            builder.Property(c => c.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

        }
    }
}
