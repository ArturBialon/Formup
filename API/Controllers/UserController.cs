using API.Controllers.Base;
using Application.Common.Results;
using Application.DTOs.Response;
using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<UserListItemResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query, CancellationToken ct)
        {
            var result = await Mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById([FromQuery] Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetUserByIdQuery(id), ct);
            return HandleResult(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser([FromQuery] Guid userId, [FromBody] UpdateUserCommand command, CancellationToken ct)
        {
            if (userId != command.UserId)
            {
                return HandleResult(AppResult<Unit>.Failure("REQUEST.ID_MISSMATCH"));
            }

            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }
    }
}
