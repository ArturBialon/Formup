using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.Request
{
    public class CaseRequestDTO
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Relation { get; set; }
        public int ForwarderId { get; set; }
    }
}
