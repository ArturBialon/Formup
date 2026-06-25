using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class InvoiceConfiguration : EntityConfiguration<Invoice>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Invoice> entity)
        {
            entity.Property(e => e.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Amount)
                .HasColumnType("decimal(12, 2)")
                .IsRequired();

            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.Property(e => e.IssueDate)
                .HasColumnType("datetime")
                .HasColumnName("Issue_Date");

            entity.Property(e => e.ServiceDate)
                .HasColumnType("datetime")
                .HasColumnName("Service_Date");

            entity.Property(e => e.Tax)
                .HasColumnType("decimal(7, 3)")
                .IsRequired();

            entity.Property(e => e.IsAbandoned)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasOne(d => d.WorkCase)
                .WithMany(p => p.Invoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Invoices_WorkCases");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.Invoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Invoices_Clients");
        }
    }
}
