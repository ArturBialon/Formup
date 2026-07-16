#nullable disable

using Domain.Entity;

namespace Domain.Models
{
    public partial class Invoice : Entity<Invoice>
    {
        public Invoice()
        {
            WorkCaseItems = new HashSet<WorkCaseItem>();
        }

        public string InvoiceNumber { get; set; } = null!;
        public decimal Tax { get; set; }
        public DateTime IssueDateUtc { get; set; }
        public DateTime ServiceDateUtc { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; } = "PLN";
        public bool IsAbandoned { get; set; } = false;
        public bool IsPaid { get; set; } = false;
        public decimal AmountInPln { get; set; }

        public virtual WorkCase WorkCase { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<WorkCaseItem> WorkCaseItems { get; set; }
    }
}
