using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CostConfiguration : EntityConfiguration<Cost>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Cost> entity)
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Tax)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.WorkCase)
                .WithMany(p => p.Costs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Costs_WorkCases");

            entity.HasOne(d => d.ServiceContractor)
                .WithMany(p => p.Costs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Costs_Service_Contractors");
        }
    }
}
