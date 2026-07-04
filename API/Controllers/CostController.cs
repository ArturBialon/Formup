using API.Controllers.Base;
using Application.Common.Results;
using Application.DTOs.Response;
using Application.Features.Costs.Commands;
using Application.Features.Costs.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CostController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<CostDetailResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCosts([FromQuery] GetCostsQuery query, CancellationToken ct)
        {
            var result = await Mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CostDetailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCostById([FromQuery] Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetCostByIdQuery(id), ct);
            return HandleResult(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCost([FromForm] CreateCostCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCost([FromForm] UpdateCostCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCost([FromQuery] Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new DeleteCostCommand(id), ct);
            return HandleResult(result);
        }
    }
}
