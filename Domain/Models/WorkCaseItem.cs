#nullable enable

using Domain.Entity;

namespace Domain.Models
{
    public partial class WorkCaseItem : Entity<WorkCaseItem>
    {
        public required string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PLN";
        public decimal Tax { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.Now;
        public bool IsInvoiced => Invoice != null;
        public bool HasCosts => Costs.Count != 0;

        public virtual Invoice? Invoice { get; set; }
        public virtual ICollection<Cost> Costs { get; set; } = [];
        public required virtual WorkCase WorkCase { get; set; }
    }
}
