using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }
        public int Tax { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public decimal Amount { get; set; }
        public int CasesId { get; set; }
        public int ClientsId { get; set; }

        public virtual Case Cases { get; set; }
        public virtual Client Clients { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
