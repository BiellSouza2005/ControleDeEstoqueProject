using ControleDeEstoqueAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeEstoqueAPI.Configurations
{
    public class ClientConfigurations : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(c => c.Id);

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
