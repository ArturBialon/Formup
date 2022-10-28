using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.Request;
using API.Enum;

namespace API.Services.Interfaces
{
    public interface ILogin
    {
        Task<UserDTO> AddForwarder(ForwarderAddDTO forwarder);
        Task<UserDTO> UserLoginStatus(UserLoginDTO user);
    }
}
