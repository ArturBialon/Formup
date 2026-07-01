using API.Controllers.Base;
using Application.Common.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/bug")]
public class BuggyController() : ApiControllerBase
{

    [Authorize]
    [HttpGet("secret")]
    [ActionName("GetSecretIfLogged")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<string> GetSecretIfLogged()
    {
        return Ok("secret text");
    }

    [HttpGet("unauthorized-custom")]
    [ActionName("Get401Unauthorized")]
    public IActionResult Get401Unauthorized()
    {
        var result = AppResult<string>.Failure("UNAUTHORIZED_ERROR");
        return HandleResult(result);
    }

    [HttpGet("not-found")]
    [ActionName("GetNotFound")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetNotFound()
    {
        var result = AppResult<string>.Failure("COMMON.NOT_FOUND");
        return HandleResult(result);
    }

    [HttpGet("bad-request")]
    [ActionName("GetBadRequest")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetBadRequest()
    {
        var result = AppResult<string>.Failure("AUTH.INVALID_CREDENTIALS");
        return HandleResult(result);
    }

    [HttpGet("server-error")]
    [ActionName("GetServerError")]
    public async Task<IActionResult> GetServerError()
    {
        var result = AppResult<string>.Failure("SERVER.UNKNOWN_ERROR");
        return HandleResult(result);
    }
}