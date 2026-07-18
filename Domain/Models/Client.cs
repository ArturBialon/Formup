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
        public required string Country { get; set; }
        public required string City { get; set; }
        public required string Zip { get; set; }
        public required string Street { get; set; }
        public required string HouseNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Credit { get; set; }
        public decimal CreditInPln { get; set; }
        public string CurrencyCode { get; set; } = "PLN";
        public bool IsActive { get; set; } = false;

        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<WorkCase> WorkCases { get; set; }

        public static bool CanAssignAmount(
            decimal requestedAmountInPln,
            decimal currentActiveUsageInPln,
            decimal clientCreditInPln,
            out decimal exceededBy)
        {
            var availableCredit = clientCreditInPln - currentActiveUsageInPln;
            if (requestedAmountInPln > availableCredit)
            {
                exceededBy = requestedAmountInPln - availableCredit;
                return false;
            }

            exceededBy = 0;
            return true;
        }
    }
}
