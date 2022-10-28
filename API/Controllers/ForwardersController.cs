using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.DTO;
using API.Services.Interfaces;
using API.DTO.Request;

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
        
        [HttpGet]
        public async Task<IActionResult> GetForwarders()
        {

            ICollection<ForwarderDTO> forwarders = await _repository.GetForwarders();

            if (forwarders != null)
                return Ok(forwarders);
            else
                return NotFound("No data");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetForwarderById(int id)
        {

            ForwarderDTO forwarder = await _repository.GetForwarderById(id);

            if (forwarder != null)
                return Ok(forwarder);
            else
                return NotFound("No records matching to given id");
        }

        [HttpPost("register")]
        public async Task<IActionResult> AddForwarder(ForwarderAddDTO forwarder)
        {

            var result = await _repository.AddForwarder(forwarder);

            if (result == "forwarder successfully added")
            {
                return Ok(result);
            }
            else
                return BadRequest(result);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditForwarder(int id, ForwarderDTO forwarder)
        {
            var result = await _repository.EditForwarder(id, forwarder);

            if (result == "forwarder successfully edited")
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

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
