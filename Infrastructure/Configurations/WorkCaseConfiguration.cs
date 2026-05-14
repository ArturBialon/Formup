using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class WorkCaseConfiguration : EntityConfiguration<WorkCase>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WorkCase> entity)
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.Property(e => e.Relation)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            entity.HasOne(d => d.Forwarder)
                .WithMany(p => p.WorkCases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkCases_Forwarders");
        }
    }
}
