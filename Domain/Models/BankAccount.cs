#nullable disable
using Domain.Entity;

namespace Domain.Models
{
    public partial class BankAccount : Entity<BankAccount>
    {
        public string BankName { get; set; }
        public string IBAN { get; set; }
        public string CurrencyCode { get; set; } = "PLN";
        public bool IsMain { get; set; } = false;

        public virtual ServiceContractor ServiceContractor { get; set; }
    }
}