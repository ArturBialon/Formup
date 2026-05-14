#nullable disable

using Domain.Entity;

namespace Domain.Models
{
    public partial class Cost : Entity<Cost>
    {
        public decimal Amount { get; set; }
        public int Tax { get; set; }
        public required string Name { get; set; }

        public virtual WorkCase WorkCase { get; set; }
        public virtual ServiceContractor ServiceProvider { get; set; }
    }
}
