using Infrastructure.Models;

namespace Domain.DTO.Request
{
    public class UserLoginDTO
    {
        public Forwarder.EntityId Id { get; set; }
        public required string Login { get; set; }
        public required string Prefix { get; set; }
        public required string Password { get; set; }
    }
}
