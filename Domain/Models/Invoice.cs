#nullable disable

using Domain.Entity;

namespace Domain.Models
{
    public partial class Invoice : Entity<Invoice>
    {
        public Invoice()
        {
            Services = new HashSet<Service>();
        }

        public int Tax { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public decimal Amount { get; set; }

        public virtual WorkCase WorkCase { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
