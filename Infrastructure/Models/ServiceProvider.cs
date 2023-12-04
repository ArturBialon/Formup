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
        public string Tax { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string Coutry { get; set; }

        public virtual ICollection<Cost> Costs { get; set; }
    }
}
