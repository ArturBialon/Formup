using API.Enum;

namespace API.DTO.Request
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public CommonEnum Status { get; set; }
    }
}
