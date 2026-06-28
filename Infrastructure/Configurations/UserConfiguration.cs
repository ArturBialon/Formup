using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserConfiguration : EntityConfiguration<User>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<User> entity)
        {
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.Prefix)
                .IsRequired()
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.Property(e => e.Surname)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.Property(e => e.PassHash)
                .IsRequired()
                .HasColumnType("varbinary")
                .IsUnicode(false);

            entity.Property(e => e.PassSalt)
                .IsRequired()
                .HasColumnType("varbinary")
                .IsUnicode(false);

            entity.Property(e => e.IsActive)
                .IsRequired();
        }
    }
}
