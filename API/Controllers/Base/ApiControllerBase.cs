using Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class ApiControllerBase() : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected IActionResult HandleResult<T>(IAppResult<T> result)
        {
            if (result.IsSuccess) return Ok(result.Value);

            if (result.Errors != null && result.Errors.Count > 0)
            {
                return BadRequest(new
                {
                    error = result.ErrorCode,
                    errors = result.Errors
                });
            }

            return BadRequest(new
            {
                error = result.ErrorCode,
                details = result.ErrorData
            });
        }
    }
}
