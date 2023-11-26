using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.Request
{
    public class ForwarderAddDTO
    {
        public required string Login { get; set; }
        public required string Surname { get; set; }
        public required string Prefix { get; set; }
        public required string PassHash { get; set; }

    }
}
