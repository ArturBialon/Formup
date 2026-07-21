using Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public abstract class ApiControllerBase() : ControllerBase
    {
        private Serilog.ILogger? _logger;
        private Serilog.ILogger Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<Serilog.ILogger>();

        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected IActionResult HandleResult<T>(AppResult<T> result)
        {
            if (result.IsSuccess) return Ok(result.Value);

            var errorCodes = new List<string>();

            if (result.Errors != null && result.Errors.Count > 0)
            {
                var validationCodes = result.Errors.Values.SelectMany(x => x);
                errorCodes.AddRange(validationCodes);
            }
            else if (!string.IsNullOrEmpty(result.ErrorCode))
            {
                errorCodes.Add(result.ErrorCode);
            }

            var requestPath = HttpContext.Request.Path.Value;
            Logger.Warning("Request to {RequestPath} failed with errors: {@ErrorCodes}", requestPath, errorCodes);

            if (errorCodes.Any(x => x.Contains("UNAUTHORIZED")))
                return Unauthorized(new { errors = errorCodes });

            if (errorCodes.Any(x => x.Contains("FORBIDDEN")))
                return StatusCode(StatusCodes.Status403Forbidden, new { errors = errorCodes });

            if (errorCodes.Any(x => x.Contains("NOT_FOUND")))
                return NotFound(new { errors = errorCodes });

            if (errorCodes.Count == 0)
                errorCodes.Add("SERVER.UNKNOWN_ERROR");

            return BadRequest(new { errors = errorCodes, data = result.ErrorData });
        }
    }
}