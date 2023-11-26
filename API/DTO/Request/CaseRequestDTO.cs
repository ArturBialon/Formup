using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTO.Request
{
    public class CaseRequestDTO
    {
        [Required]
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Relation { get; set; }
        [Required]
        public int ForwarderId { get; set; }
    }
}
