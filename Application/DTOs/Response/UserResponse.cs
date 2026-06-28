namespace Application.DTOs.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public required string Role { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required ResponseToken Token { get; set; }
    }
}
