using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
    public class ClientDTO
    {
        public string Tax { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string Coutry { get; set; }
        public decimal Credit { get; set; }
    }
}
