using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Models
{
    public partial class Forwarder
    {
        public Forwarder()
        {
            Cases = new HashSet<Case>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Prefix { get; set; }
        public byte[] PassHash { get; set; }
        public byte[] PassSalt { get; set; }

        public virtual ICollection<Case> Cases { get; set; }
    }
}
