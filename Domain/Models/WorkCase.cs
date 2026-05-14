
#nullable disable

using Domain.Entity;

namespace Domain.Models
{
    public partial class WorkCase : Entity<WorkCase>
    {
        public WorkCase()
        {
            Costs = new HashSet<Cost>();
            Invoices = new HashSet<Invoice>();
        }

        public required string Name { get; set; }
        public int Amount { get; set; }
        public required string Relation { get; set; }

        public virtual Forwarder Forwarder { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
