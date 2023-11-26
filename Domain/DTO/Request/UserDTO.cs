using Domain.Enum;

namespace Domain.DTO.Request
{
    public class UserDTO
    {
        public required string UserName { get; set; }
        public required string Token { get; set; }
        public CommonEnum Status { get; set; }
    }
}
