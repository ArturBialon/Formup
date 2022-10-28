using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interfaces;
using API.DTO.Request;

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
        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginDTO user)
        {
            var status = await _repository.UserLogin(user);
            if (status == Enum.CommonEnum.SUCCESSFULLY_FOUND)
                return Ok("You are logged into system");
            else if (status == Enum.CommonEnum.INVALID_LOGIN)
                return Unauthorized("Login invalid");
            else if (status == Enum.CommonEnum.INVALID_PASSWORD)
                return Unauthorized("Password invalid");
            else
                return BadRequest("Error occoured " + status.ToString());
        }
    }
}
