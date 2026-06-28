#nullable disable

using Domain.Entity;

namespace Domain.Models
{
    public partial class Client : Entity<Client>
    {
        public Client()
        {
            Invoices = new HashSet<Invoice>();
            WorkCases = new HashSet<WorkCase>();
        }

        public required string Tax { get; set; }
        public required string Name { get; set; }
        public required string Street { get; set; }
        public required string Zip { get; set; }
        public required string Coutry { get; set; }
        public decimal Credit { get; set; }
        public string Currency { get; set; } = "PLN";

        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<WorkCase> WorkCases { get; set; }

        public bool CanAssignAmount(decimal requestedAmount, decimal currentActiveUsage, out decimal exceededBy)
        {
            var availableCredit = Credit - currentActiveUsage;
            if (requestedAmount > availableCredit)
            {
                exceededBy = requestedAmount - availableCredit;
                return false;
            }

            exceededBy = 0;
            return true;
        }
    }
}
