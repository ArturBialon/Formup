using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ClientConfiguration : EntityConfiguration<Client>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Client> entity)
        {
            entity.Property(e => e.Coutry)
                    .IsRequired()
                    .HasMaxLength(54)
                    .IsUnicode(false);

            entity.Property(e => e.Credit)
                .HasColumnType("decimal(12, 2)");

            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(3)
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
                .IsUnicode(false);

            entity.Property(e => e.Zip)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);
        }
    }
}
