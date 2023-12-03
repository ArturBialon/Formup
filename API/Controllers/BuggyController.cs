using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/bug")]
    public class BuggyController : ControllerBase
    {
        private readonly FormupContext _context;
        public BuggyController(FormupContext compContext)
        {
            _context = compContext;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return Ok("secret text");
        }

        [HttpGet("not-found")]
        public ActionResult<Forwarder> GetNotFound()
        {
            var thing = _context.Forwarders.Find(-1);
            if (thing == null) return NotFound();
            return Ok();
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {

            var thing = _context.Forwarders.Find(-1);
            var thingToReturn = thing.ToString();

            return thingToReturn;

        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("not gooood bro");
        }
    }
}
