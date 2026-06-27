namespace Application.DTOs.Response
{
    public class UserListItemResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string FullName => $"{Name} {Surname}";
        public string Prefix { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
