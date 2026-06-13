namespace Application.DTOs.Response
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required ResponseTokenDTO Token { get; set; }
    }
}
