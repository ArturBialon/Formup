using Application.Controllers.Base;
using Domain.DTO;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    //[Authorize]
    public class ClientController : ApiControllerBase
    {
        public readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {

            ICollection<ClientDTO> clients = await _clientService.GetClients();

            if (clients.Count != 0)
                return Ok(clients);
            else
                return NotFound("No data");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {

            var client = await _clientService.GetClientById(id);

            if (client != null)
                return Ok(client);
            else
                return NotFound("No records matching to given id");
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(ClientDTO client)
        {
            var response = await _clientService.AddClient(client);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditClient(ClientDTO client)
        {
            var response = await _clientService.EditClient(client);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var response = await _clientService.DeleteClientById(id);
            return Ok(response);
        }
    }
}
