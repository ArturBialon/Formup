using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.Request
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
