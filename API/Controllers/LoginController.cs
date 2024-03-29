﻿using Application.Controllers.Base;
using Application.Services.Interfaces;
using Domain.DTO.Request;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Controllers
{
    public class LoginController : ApiControllerBase
    {
        public readonly ILogin _repository;
        public LoginController(ILogin forwardersRep)
        {
            _repository = forwardersRep;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginDTO user)
        {
            var userLoged = await _repository.UserLoginStatus(user);

            if (userLoged.Status == CommonEnum.SUCCESSFULLY_FOUND)
                return Ok(userLoged);
            if (userLoged.Status == CommonEnum.INVALID_LOGIN)
                return Unauthorized("Login invalid");
            if (userLoged.Status == CommonEnum.INVALID_PASSWORD)
                return Unauthorized("Password invalid");
            else
                return BadRequest("Error occoured " + userLoged.Status.ToString());
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> AddForwarder(ForwarderAddDTO forwarder)
        {

            var newUser = await _repository.AddForwarder(forwarder);

            if (newUser.Status == CommonEnum.SUCCESSFULLY_ADDED)
                return Ok(newUser);
            if (newUser.Status == CommonEnum.INVALID_LOGIN)
                return Unauthorized("Login is taken");
            if (newUser.Status == CommonEnum.ALREADY_EXISTS)
                return BadRequest("This forwarder alredy exists");
            if (newUser.Status == CommonEnum.INVALID_PASSWORD)
                return BadRequest("Password invalid - must be at least 6 characters");
            else
                return BadRequest("Error occoured \n" + newUser.Status.ToString());

        }
    }
}
