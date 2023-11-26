using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTO.Request
{
    public class ForwarderAddDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Prefix { get; set; }
        [Required]
        public string PassHash { get; set; }

    }
}
