using API.Controllers.Base;
using Application.DTOs.Response;
using Application.Features.Forwarders.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthController() : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(LoginCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(RegisterForwarderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
