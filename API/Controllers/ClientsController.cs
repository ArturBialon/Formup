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
    }
}
