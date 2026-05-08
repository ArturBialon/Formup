#nullable disable

using Infrastructure.Entity;

namespace Infrastructure.Models
{
    public partial class ServiceProvider : Entity<ServiceProvider>
    {
        public ServiceProvider()
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
