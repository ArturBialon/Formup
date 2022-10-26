using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.Request
{
    public class ForwarderAddDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Prefix { get; set; }
        public string PassHash { get; set; }

    }
}
