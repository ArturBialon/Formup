using Application.Controllers.Base;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    //[Authorize]
    public class ForwarderController : ApiControllerBase
    {
        public readonly IForwarderService _forwarderService;
        public ForwarderController(IForwarderService forwarderService)
        {
            _forwarderService = forwarderService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetForwarders()
        {

            ICollection<ForwarderResponseDTO> forwarders = await _forwarderService.GetForwarders();

            if (forwarders != null)
                return Ok(forwarders);
            else
                return NotFound("No data");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetForwarderById(int id)
        {

            ForwarderResponseDTO forwarder = await _forwarderService.GetForwarderById(id);

            if (forwarder != null)
                return Ok(forwarder);
            else
                return NotFound("No records matching to given id");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditForwarder(ForwarderRequestDTO forwarder)
        {
            var response = await _forwarderService.EditForwarder(forwarder);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForwarder(int id)
        {
            var response = await _forwarderService.DeleteForwarderById(id);
            return Ok(response);
        }

    }
}
