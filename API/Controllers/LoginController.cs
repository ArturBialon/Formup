using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interfaces;
using API.DTO.Request;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/Login")]
    public class LoginController : ControllerBase
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

            if (userLoged.Status == Enum.CommonEnum.SUCCESSFULLY_FOUND)
                return Ok(userLoged);
            else if (userLoged.Status == Enum.CommonEnum.INVALID_LOGIN)
                return Unauthorized("Login invalid");
            else if (userLoged.Status == Enum.CommonEnum.INVALID_PASSWORD)
                return Unauthorized("Password invalid");
            else
                return BadRequest("Error occoured " + userLoged.Status.ToString());
        }

        //[Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> AddForwarder(ForwarderAddDTO forwarder)
        {

            var newUser = await _repository.AddForwarder(forwarder);

            if (newUser.Status == Enum.CommonEnum.SUCCESSFULLY_FOUND)
                return Ok("You are signed into system");
            else if (newUser.Status == Enum.CommonEnum.INVALID_LOGIN)
                return Unauthorized("Login is taken");
            else if (newUser.Status == Enum.CommonEnum.ALREADY_EXISTS)
                return BadRequest("This forwarder alredy exists");
            else if (newUser.Status == Enum.CommonEnum.INVALID_PASSWORD)
                return BadRequest("Password invalid - must be at least 6 chars");
            else
                return BadRequest("Error occoured \n" + newUser.Status.ToString());

        }
    }
}
