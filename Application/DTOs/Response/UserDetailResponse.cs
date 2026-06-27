namespace Application.DTOs.Response
{
    public class UserDetailResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Prefix { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
