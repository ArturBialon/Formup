namespace Domain.DTO.Request
{
    public class UserLoginDTO
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string Prefix { get; set; }
        public required string Password { get; set; }
    }
}
