using Application.Controllers.Base;
using Domain.Interfaces.Repository;
using Domain.DTO.Request;
using Domain.DTO.Response;
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

            ICollection<ForwarderResponseDTO> forwarders = await _repository.GetForwarders();

            if (forwarders != null)
                return Ok(forwarders);
            else
                return NotFound("No data");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetForwarderById(int id)
        {

            ForwarderResponseDTO forwarder = await _repository.GetForwarderById(id);

            if (forwarder != null)
                return Ok(forwarder);
            else
                return NotFound("No records matching to given id");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditForwarder(int id, ForwarderRequestDTO forwarder)
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
