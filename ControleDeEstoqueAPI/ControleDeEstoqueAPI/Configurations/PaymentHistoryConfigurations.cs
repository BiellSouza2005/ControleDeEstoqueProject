using ControleDeEstoqueAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeEstoqueAPI.Configurations
{
    public class PaymentHistoryConfigurations : IEntityTypeConfiguration<PaymentHistory>
    {
        public void Configure(EntityTypeBuilder<PaymentHistory> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasKey(c => c.Id);

            builder.Property(ph => ph.PreviousAmount)
                .HasPrecision(18, 2);

            builder.Property(ph => ph.NewAmount)
                .HasPrecision(18, 2);

            builder.Property(c => c.ModificationDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

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
