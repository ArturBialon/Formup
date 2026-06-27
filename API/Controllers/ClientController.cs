using API.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ClientController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:guid}")]
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

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
