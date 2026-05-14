using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ServiceConfiguration : EntityConfiguration<Service>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Service> entity)
        {
            entity.Property(e => e.Amonut).HasColumnType("decimal(12, 2)");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.Tax).HasColumnName("TAX");

            entity.HasOne(d => d.Invoice)
                .WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Service_Invoices");
        }
    }
}
