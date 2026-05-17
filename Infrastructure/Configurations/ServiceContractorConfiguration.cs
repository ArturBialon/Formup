using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ServiceContractorConfiguration : EntityConfiguration<ServiceContractor>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ServiceContractor> entity)
        {
            entity.Property(e => e.Coutry)
                    .IsRequired()
                    .HasMaxLength(54)
                    .IsUnicode(false);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Street)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.Property(e => e.Tax)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TAX");

            entity.Property(e => e.Zip)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ZIP");
        }
    }
}
