
namespace Domain.DTO.Response
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Token { get; set; }
    }
}
