using API.Controllers.Base;
using Application.Common.Results;
using Application.DTOs.Response;
using Application.Features.ServiceContractors.Commands;
using Application.Features.ServiceContractors.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ServiceContractorController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ServiceContractorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServiceContractors([FromQuery] GetServiceContractorsQuery query, CancellationToken ct)
        {
            var result = await Mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceContractorById([FromQuery] Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetServiceContractorByIdQuery(id), ct);
            return HandleResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceContractorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddServiceContractor([FromBody] CreateServiceContractorCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditServiceContractor([FromBody] UpdateServiceContractorCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteServiceContractor([FromQuery] Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new DeleteServiceContractorCommand(id), ct);
            return HandleResult(result);
        }
    }
}
