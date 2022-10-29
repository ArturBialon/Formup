using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.DTO;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/Clients")]
    public class ClientsController : ControllerBase
    {
        public readonly IClientDbRepository _repository;
        public ClientsController(IClientDbRepository clientRep)
        {
            _repository = clientRep;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {

            ICollection<ClientDTO> clients = await _repository.GetClients();

            if (clients.Count != 0)
                return Ok(clients);
            else
                return NotFound("No data");
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {

            ClientDTO client = await _repository.GetClientById(id);

            if (client != null)
                return Ok(client);
            else
                return NotFound("No records matching to given id");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddClient(ClientDTO client)
        {

            var result = await _repository.AddClient(client);

            if (result == Enum.CommonEnum.SUCCESSFULLY_ADDED)
            {
                return Ok("contractor successfully added");
            }
            else
                return BadRequest(result.ToString());
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditClient(int id, ClientDTO client)
        {
            var result = await _repository.EditClient(id, client);

            if (result == Enum.CommonEnum.CHANGES_SAVED)
            {
                return Ok("contractor successfully edited");
            }
            else
                return BadRequest(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _repository.DeleteClientById(id);

            if (result == Enum.CommonEnum.SUCCESSFULLY_REMOVED)
            {
                return Ok("contractor removed");
            }
            else
                return BadRequest(result);
        }
    }
}
