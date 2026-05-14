#nullable disable

using Domain.Entity;

namespace Domain.Models
{
    public partial class Service : Entity<Service>
    {
        public required string Name { get; set; }
        public decimal Amonut { get; set; }
        public int Tax { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
