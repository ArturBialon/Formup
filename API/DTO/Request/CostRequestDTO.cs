using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.Request
{
    public class CostRequestDTO
    {
        public decimal Amount { get; set; }
        public int Tax { get; set; }
        public string Name { get; set; }
        public int CasesId { get; set; }
        public int ServiceProvidersId { get; set; }
    }
}
