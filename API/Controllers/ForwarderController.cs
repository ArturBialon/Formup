using API.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    public class ForwarderController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetForwarders()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetForwarderById(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditForwarder()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> SoftDeleteForwarder(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}
