using Application.Controllers.Base;
using Application.DTOs.Response;
using Application.Features.Forwarders.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Controllers
{
    public class AuthController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator) => _mediator = mediator;

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDTO>> Login(LoginCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDTO>> Register(RegisterForwarderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
