#nullable disable

namespace Infrastructure.Models
{
    public partial class Client
    {
        public Client()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        public string Tax { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string Coutry { get; set; }
        public decimal Credit { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
