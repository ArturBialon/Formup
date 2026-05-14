#nullable disable

using Domain.Entity;

namespace Domain.Models
{
    public partial class ServiceContractor : Entity<ServiceContractor>
    {
        public ServiceContractor()
        {
            Costs = new HashSet<Cost>();
        }
        public required string Tax { get; set; }
        public required string Name { get; set; }
        public required string Street { get; set; }
        public required string Zip { get; set; }
        public required string Coutry { get; set; }

        public virtual ICollection<Cost> Costs { get; set; }
    }
}
