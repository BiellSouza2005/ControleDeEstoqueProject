using ControleDeEstoqueAPI.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoqueAPI.Configurations
{
    public class ProductDescriptionConfigurations : IEntityTypeConfiguration<ProductDescription>
    {
        public void Configure(EntityTypeBuilder<ProductDescription> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

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
