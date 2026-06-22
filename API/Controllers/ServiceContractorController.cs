using API.Controllers.Base;
using Application.Common.Models;
using Application.DTOs.Response;
using Application.Features.ServiceContractors.Commands;
using Application.Features.ServiceContractors.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    public class ServiceContractorController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ServiceContractorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServiceContractors([FromQuery] GetServiceContractorsQuery query, CancellationToken ct)
        {
            var result = await Mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ServiceContractorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServiceContractorById(Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetServiceContractorByIdQuery(id), ct);
            return HandleResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceContractorResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddServiceContractor([FromBody] CreateServiceContractorCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            if (result.IsFailure) return HandleResult(result);

            return CreatedAtAction(nameof(GetServiceContractorById), new { id = result.Value!.Id }, result.Value);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ServiceContractorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditServiceContractor([FromBody] UpdateServiceContractorCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteServiceContractor(Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new DeleteServiceContractorCommand(id), ct);
            return HandleResult(result);
        }
    }
}
