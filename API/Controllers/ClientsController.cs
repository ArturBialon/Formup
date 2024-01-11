using Application.Controllers.Base;
using Domain.Interfaces.Repository;
using Domain.DTO;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [Authorize]
    public class ClientsController : ApiControllerBase
    {
        public readonly IClientDbRepository _repository;
        public ClientsController(IClientDbRepository clientRep)
        {
            _repository = clientRep;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {

            ICollection<ClientRquestDTO> clients = await _repository.GetClients();

            if (clients.Count != 0)
                return Ok(clients);
            else
                return NotFound("No data");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {

            ClientRquestDTO client = await _repository.GetClientById(id);

            if (client != null)
                return Ok(client);
            else
                return NotFound("No records matching to given id");
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(ClientRquestDTO client)
        {

            var response = await _repository.AddClient(client);

            if (response == CommonEnum.SUCCESSFULLY_ADDED)
            {
                return Ok("contractor successfully added");
            }
            else
                return BadRequest(response.ToString());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditClient(int id, ClientRquestDTO client)
        {
            var response = await _repository.EditClient(id, client);

            if (response == CommonEnum.CHANGES_SAVED)
            {
                return Ok("contractor successfully edited");
            }
            else
                return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var response = await _repository.DeleteClientById(id);

            if (response == CommonEnum.SUCCESSFULLY_REMOVED)
            {
                return Ok("contractor removed");
            }
            else
                return BadRequest(response);
        }
    }
}
