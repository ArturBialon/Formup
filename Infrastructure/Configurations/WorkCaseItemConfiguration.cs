using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class WorkCaseItemConfiguration : EntityConfiguration<WorkCaseItem>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WorkCaseItem> entity)
        {
            entity.Property(e => e.AmountToInvoice)
                .HasColumnType("decimal(12, 2)")
                .IsRequired();

            entity.Property(e => e.CurrencyCodeInvoice)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(true);

            entity.Property(e => e.TaxInvoice)
                .HasColumnType("decimal(7, 3)")
                .IsRequired();

            entity.Property(e => e.CostAmountNet)
                .HasColumnType("decimal(12, 2)")
                .IsRequired();

            entity.Property(e => e.CurrencyCodeCost)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.Property(e => e.CreatedAtUtc)
                .IsRequired()
                .HasColumnType("datetime");

            entity.HasOne(d => d.Invoice)
                .WithMany(p => p.WorkCaseItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkCaseItems_Invoices");

            entity.HasOne(d => d.WorkCase)
                .WithMany(p => p.WorkCaseItems)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_WorkCaseItems_WorkCases");
        }
    }
}
