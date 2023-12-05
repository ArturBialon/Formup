
#nullable disable

namespace Infrastructure.Models
{
    public partial class Case
    {
        public Case()
        {
            Costs = new HashSet<Cost>();
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        public required string Name { get; set; }
        public int Amount { get; set; }
        public required string Relation { get; set; }
        public int ForwardersId { get; set; }

        public virtual Forwarder Forwarders { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
