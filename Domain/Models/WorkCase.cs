#nullable disable

using Domain.Entity;

namespace Domain.Models
{
    public partial class WorkCase : Entity<WorkCase>
    {
        public WorkCase()
        {
            Invoices = new HashSet<Invoice>();
        }

        public required string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountInPln { get; set; }
        public string CurrencyCode { get; set; } = "PLN";
        public required string Relation { get; set; }
        public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
        public bool IsAbandoned { get; set; } = false;
        public bool IsCompleted { get; set; } = false;

        public virtual User Forwarder { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<WorkCaseItem> WorkCaseItems { get; set; }
    }
}
