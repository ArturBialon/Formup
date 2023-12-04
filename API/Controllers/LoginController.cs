using Domain.DTO.Request;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain.Enum;
using Application.Controllers.Base;

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
            else if (userLoged.Status == CommonEnum.INVALID_LOGIN)
                return Unauthorized("Login invalid");
            else if (userLoged.Status == CommonEnum.INVALID_PASSWORD)
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
            else if (newUser.Status == CommonEnum.INVALID_LOGIN)
                return Unauthorized("Login is taken");
            else if (newUser.Status == CommonEnum.ALREADY_EXISTS)
                return BadRequest("This forwarder alredy exists");
            else if (newUser.Status == CommonEnum.INVALID_PASSWORD)
                return BadRequest("Password invalid - must be at least 6 chars");
            else
                return BadRequest("Error occoured \n" + newUser.Status.ToString());

        }
    }
}
