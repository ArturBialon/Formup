#nullable enable

using Domain.Entity;

namespace Domain.Models
{
    public partial class Cost : Entity<Cost>
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PLN";
        public decimal Tax { get; set; }
        public required string Name { get; set; }
        public required DateTime IssueDate { get; set; }
        public required DateTime ServiceDate { get; set; }
        public string? DocumentUrl { get; set; }

        public required virtual WorkCaseItem WorkCaseItem { get; set; }
        public required virtual ServiceContractor ServiceContractor { get; set; }
    }
}
