namespace Domain.DTO.Request
{
    public class UserLoginDTO
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}
