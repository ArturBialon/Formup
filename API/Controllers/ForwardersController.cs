using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.DTO;
using API.Services.Interfaces;
using API.DTO.Request;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/Forwarders")]
    public class ForwardersController : ControllerBase
    {
        public readonly IForwardersDbRepository _repository;
        public ForwardersController(IForwardersDbRepository forwardersRep)
        {
            _repository = forwardersRep;
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetForwarders()
        {

            ICollection<ForwarderDTO> forwarders = await _repository.GetForwarders();

            if (forwarders != null)
                return Ok(forwarders);
            else
                return NotFound("No data");
        }

        //[Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForwarderById(int id)
        {

            ForwarderDTO forwarder = await _repository.GetForwarderById(id);

            if (forwarder != null)
                return Ok(forwarder);
            else
                return NotFound("No records matching to given id");
        }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditForwarder(int id, ForwarderAddDTO forwarder)
        {
            var result = await _repository.EditForwarder(id, forwarder);

            if (result == "forwarder successfully edited")
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForwarder(int id)
        {
            var result = await _repository.DeleteForwarderById(id);

            if (result == "forwarder removed")
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        
    }
}
