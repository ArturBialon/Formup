using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ServiceContractorConfiguration : EntityConfiguration<ServiceContractor>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ServiceContractor> entity)
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(true);

            entity.Property(e => e.Tax)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(54)
                .IsUnicode(true);

            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true);

            entity.Property(e => e.Zip)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Street)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true);

            entity.Property(e => e.HouseNumber)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.ApartmentNumber)
                .IsRequired(false)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Email)
                .IsRequired(false)
                .HasMaxLength(254)
                .IsUnicode(false);

            entity.Property(e => e.PhoneNumber)
                .IsRequired(false)
                .HasMaxLength(20)
                .IsUnicode(false);
        }
    }
}
