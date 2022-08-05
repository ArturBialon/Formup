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
        public async Task<IActionResult> GetOsobyDoKontaktu()
        {

            ICollection<ClientDTO> clients = await _repository.GetClients();

            return Ok(clients);
        }
    }
}
