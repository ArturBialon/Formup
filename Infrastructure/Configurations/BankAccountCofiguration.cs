using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BankAccountConfiguration : EntityConfiguration<BankAccount>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<BankAccount> entity)
        {
            entity.ToTable("BankAccounts");

            entity.Property(e => e.BankName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.IBAN)
                .IsRequired()
                .HasMaxLength(34)
                .IsUnicode(false);

            entity.Property(e => e.CurrencyCode)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("PLN");

            entity.Property(e => e.IsMain)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasOne(d => d.ServiceContractor)
                .WithMany(p => p.BankAccounts)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BankAccounts_ServiceContractors");
        }
    }
}