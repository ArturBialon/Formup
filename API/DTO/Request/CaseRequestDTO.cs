using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.Request
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
