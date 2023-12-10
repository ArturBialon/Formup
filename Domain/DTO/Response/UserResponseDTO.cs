namespace Domain.DTO.Response
{
    public class UserResponseDTO
    {
        public required string UserName { get; set; }
        public required string Token { get; set; }
        public required string ErrorMessage { get; set; }
    }
}
