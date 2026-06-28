using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class WorkCaseItemConfiguration : EntityConfiguration<WorkCaseItem>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WorkCaseItem> entity)
        {
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(12, 2)")
                .IsRequired();

            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(true);

            entity.Property(e => e.Tax)
                .HasColumnType("decimal(7, 3)")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
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
