#nullable disable

using Infrastructure.Entity;

namespace Infrastructure.Models
{
    public partial class Cost : Entity<Cost>
    {
        public decimal Amount { get; set; }
        public int Tax { get; set; }
        public required string Name { get; set; }

        public virtual WorkCase WorkCase { get; set; }
        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}
