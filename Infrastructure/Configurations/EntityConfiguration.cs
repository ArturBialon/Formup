using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity<TEntity>
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasConversion(
                id => id.Value,
                value => new Entity<TEntity>.EntityId(value)
            );
            ConfigureEntity(builder);
        }
        protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}
