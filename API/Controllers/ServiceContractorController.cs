using API.Controllers.Base;
using Application.DTOs.Response;
using Application.Features.ServiceContractors.Commands;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    public class ServiceContractorController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProviders()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProviderById(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceContractorResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddServiceContractor([FromBody] CreateServiceContractorCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> EditProvider()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(ServiceContractor.EntityId id)
        {
            throw new NotImplementedException();
        }
    }
}
