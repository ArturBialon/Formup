#nullable disable

namespace API.Models
{
    public partial class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amonut { get; set; }
        public int Tax { get; set; }
        public int InvoicesId { get; set; }

        public virtual Invoice Invoices { get; set; }
    }
}
