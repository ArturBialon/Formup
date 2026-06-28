using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CostConfiguration : EntityConfiguration<Cost>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Cost> entity)
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
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Tax)
                .HasColumnType("decimal(7, 3)")
                .IsRequired();

            entity.Property(e => e.IssueDate)
                .HasColumnType("date")
                .IsRequired();

            entity.Property(e => e.ServiceDate)
                .HasColumnType("date")
                .IsRequired();

            entity.Property(e => e.DocumentUrl)
                .HasMaxLength(2048)
                .IsRequired(false);

            entity.HasOne(d => d.WorkCaseItem)
                .WithMany(p => p.Costs)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Costs_WorkCaseItems");

            entity.HasOne(d => d.ServiceContractor)
                .WithMany(p => p.Costs)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Costs_Service_Contractors");
        }
    }
}
