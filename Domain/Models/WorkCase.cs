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
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PLN";
        public required string Relation { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public bool IsAbandoned { get; set; } = false;

        public virtual Forwarder Forwarder { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<WorkCaseItem> WorkCaseItems { get; set; }
    }
}
