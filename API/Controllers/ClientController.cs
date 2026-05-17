using Application.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    //[Authorize]
    public class ClientController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> AddClient()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditClient()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
