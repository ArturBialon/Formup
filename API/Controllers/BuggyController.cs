using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/bug")]
    public class BuggyController : ControllerBase
    {
        private readonly FWD_CompContext _context;
        public BuggyController(FWD_CompContext compContext)
        {
            _context = compContext;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return Ok("secret text");
        }

        //[Authorize]
        [HttpGet("not-found")]
        public ActionResult<Forwarder> GetNotFound()
        {
            var thing = _context.Forwarders.Find(-1);
            if (thing == null) return NotFound();
            return Ok();
        }

        //[Authorize]
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {

            var thing = _context.Forwarders.Find(-1);
            var thingToReturn = thing.ToString();

            return thingToReturn;

        }

        //[Authorize]
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("not gooood bro");
        }
    }
}
