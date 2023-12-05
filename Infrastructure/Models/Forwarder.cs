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
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Prefix { get; set; }
        public byte[] PassHash { get; set; }
        public byte[] PassSalt { get; set; }

        public virtual ICollection<Case> Cases { get; set; }
    }
}
