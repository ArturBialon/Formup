namespace Domain.Entity
{
    public abstract class Entity<TEntity>
    {
        public EntityId Id { get; init; } = new(Guid.CreateVersion7());
        protected Entity() { }

        public readonly record struct EntityId : IComparable<EntityId>
        {
            public Guid Value { get; }
            public EntityId(Guid value)
            {
                Value = value;
            }

            public readonly int CompareTo(EntityId other) => Value.CompareTo(other.Value);

            public static implicit operator Guid(EntityId value) => value.Value;
            public static implicit operator EntityId(Guid entityId) => new(entityId);
        }
    }
}
