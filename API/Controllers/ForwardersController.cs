using Application.Controllers.Base;
using Application.Services.Interfaces;
using Domain.DTO;
using Domain.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [Authorize]
    public class ForwardersController : ApiControllerBase
    {
        public readonly IForwardersDbRepository _repository;
        public ForwardersController(IForwardersDbRepository forwardersRep)
        {
            _repository = forwardersRep;
        }

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetForwarderById(int id)
        {

            ForwarderDTO forwarder = await _repository.GetForwarderById(id);

            if (forwarder != null)
                return Ok(forwarder);
            else
                return NotFound("No records matching to given id");
        }

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
