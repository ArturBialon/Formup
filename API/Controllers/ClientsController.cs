using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.DTO;
using API.Services.Interfaces;

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

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {

            ICollection<ClientDTO> clients = await _repository.GetClients();

            if (clients != null)
                return Ok(clients);
            else
                return BadRequest("Brak rekorów w bazie");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {

            ClientDTO client = await _repository.GetClientById(id);

            if (client != null)
                return Ok(client);
            else
                return BadRequest("Brak rekordu o podanym id");
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(ClientDTO client)
        {

            var result = await _repository.AddClient(client);

            if (result == "contractor successfully added")
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditClient(int id, ClientDTO client)
        {
            var result = await _repository.EditClient(id, client);

            if (result == "contractor successfully edited")
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _repository.DeleteClientById(id);

            if (result == "contractor removed")
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
    }
}
