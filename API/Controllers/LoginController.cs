﻿using Application.Controllers.Base;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.UserAccessService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Controllers
{
    public class LoginController : ApiControllerBase
    {
        public readonly ILoginService _loginService;
        public LoginController(ILoginService forwardersRep)
        {
            _loginService = forwardersRep;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<UserResponseDTO> UserLogin(UserLoginDTO user)
        {
            var userLoged = await _loginService.UserLoginStatus(user);
            return userLoged;
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<UserResponseDTO> AddForwarder(ForwarderRequestDTO forwarder)
        {

            var newUser = await _loginService.AddForwarder(forwarder);
            return newUser;

        }
    }
}
