using API.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.Request
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public CommonEnum Status { get; set; }
    }
}
