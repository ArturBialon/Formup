#nullable enable

using Domain.Entity;

namespace Domain.Models
{
    public partial class WorkCaseItem : Entity<WorkCaseItem>
    {
        public required string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PLN";
        public int Tax { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.Now;
        public bool IsInvoiced { get; set; } = false;

        public virtual Invoice? Invoice { get; set; }
        public required virtual WorkCase WorkCase { get; set; }
    }
}
