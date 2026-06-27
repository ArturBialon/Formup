#nullable disable

using Domain.Entity;
using Domain.Enums;

namespace Domain.Models
{
    public partial class User : Entity<User>
    {
        public User()
        {
            WorkCases = new HashSet<WorkCase>();
        }

        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Prefix { get; set; }
        public required UserRole Role { get; set; }
        public byte[] PassHash { get; set; }
        public byte[] PassSalt { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<WorkCase> WorkCases { get; set; }
        public string FullName => $"{Name} {Surname}";
    }
}
