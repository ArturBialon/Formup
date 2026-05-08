#nullable disable

using Infrastructure.Entity;

namespace Infrastructure.Models
{
    public partial class Forwarder : Entity<Forwarder>
    {
        public Forwarder()
        {
            WorkCases = new HashSet<WorkCase>();
        }

        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Prefix { get; set; }
        public byte[] PassHash { get; set; }
        public byte[] PassSalt { get; set; }

        public virtual ICollection<WorkCase> WorkCases { get; set; }
    }
}
