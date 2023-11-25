using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTO.Request
{
    public class UserLoginDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
