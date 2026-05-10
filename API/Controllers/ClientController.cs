using Application.Controllers.Base;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> AddClient(ClientDTO client)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditClient(ClientDTO client)
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
