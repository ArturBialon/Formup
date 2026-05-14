using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class InvoiceConfiguration : EntityConfiguration<Invoice>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Invoice> entity)
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

            entity.Property(e => e.IssueDate)
                .HasColumnType("date")
                .HasColumnName("Issue_Date");

            entity.Property(e => e.ServiceDate)
                .HasColumnType("date")
                .HasColumnName("Service_Date");

            entity.Property(e => e.Tax).HasColumnName("TAX");

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
