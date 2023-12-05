#nullable disable

namespace Infrastructure.Models
{
    public partial class ServiceProvider
    {
        public ServiceProvider()
        {
            Costs = new HashSet<Cost>();
        }

        public int Id { get; set; }
        public required string Tax { get; set; }
        public required string Name { get; set; }
        public required string Street { get; set; }
        public required string Zip { get; set; }
        public required string Coutry { get; set; }

        public virtual ICollection<Cost> Costs { get; set; }
    }
}
