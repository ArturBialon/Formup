﻿using Domain.DTO.Request;
using Domain.DTO.Response;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ILogin
    {
        Task<UserDTO> AddForwarder(ForwarderAddDTO forwarder);
        Task<UserDTO> UserLoginStatus(UserLoginDTO user);
    }
}
