using API.Controllers.Base;
using Application.Features.Forwarders.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterForwarderCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
    }
}
