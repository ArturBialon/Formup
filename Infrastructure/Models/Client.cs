#nullable disable

using Infrastructure.Entity;

namespace Infrastructure.Models
{
    public partial class Client : Entity<Client>
    {
        public Client()
        {
            Invoices = new HashSet<Invoice>();
        }

        public required string Tax { get; set; }
        public required string Name { get; set; }
        public required string Street { get; set; }
        public required string Zip { get; set; }
        public required string Coutry { get; set; }
        public decimal Credit { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
