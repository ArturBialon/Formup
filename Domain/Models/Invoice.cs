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

        public decimal Tax { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PLN";

        public virtual WorkCase WorkCase { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<WorkCaseItem> WorkCaseItems { get; set; }
    }
}
