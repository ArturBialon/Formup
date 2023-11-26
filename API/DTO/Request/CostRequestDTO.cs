using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTO.Request
{
    public class CostRequestDTO
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int Tax { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CasesId { get; set; }
        [Required]
        public int ServiceProvidersId { get; set; }
    }
}
