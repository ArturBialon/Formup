using API.Controllers.Base;
using Application.Common.Models;
using Application.DTOs.Response;
using Application.Features.WorkCaseItems.Commands;
using Application.Features.WorkCaseItems.Queries;
using Application.Features.WorkCases.Commands;
using Application.Features.WorkCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    public class WorkCaseController : ApiControllerBase
    {
        #region WorkCase Operations

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<WorkCaseList>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkCases([FromQuery] GetWorkCasesQuery query, CancellationToken ct)
        {
            var result = await Mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WorkCaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkCaseById([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetWorkCaseByIdQuery(id), ct);
            return HandleResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(WorkCaseResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddWorkCase([FromBody] CreateWorkCaseCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);

            if (result.IsFailure) return HandleResult(result);

            return CreatedAtAction(nameof(GetWorkCaseById), new { id = result.Value!.Id }, result.Value);
        }

        [HttpPut]
        [ProducesResponseType(typeof(WorkCaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditWorkCase([FromBody] UpdateWorkCaseCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpPatch("{id:guid}")]
        [ProducesResponseType(typeof(WorkCaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AbandonWorkCase([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new AbandonWorkCaseCommand(id), ct);
            return HandleResult(result);
        }

        #endregion

        #region WorkCase Items Operations

        [HttpGet("items/{workCaseId:guid}")]
        [ProducesResponseType(typeof(IReadOnlyCollection<WorkCaseItemResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItemsForWorkCase([FromRoute] Guid workCaseId, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetWorkCaseItemsQuery(workCaseId), ct);
            return HandleResult(result);
        }

        [HttpPost("items/{workCaseId:guid}")]
        [ProducesResponseType(typeof(WorkCaseItemResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddItemToWorkCase([FromRoute] Guid workCaseId, [FromBody] AddWorkCaseItemCommand command, CancellationToken ct)
        {
            if (workCaseId != command.WorkCaseId)
                return BadRequest("Mismatched WorkCase identifier between URL and body.");

            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpPut("items/{workCaseItemId:guid}")]
        [ProducesResponseType(typeof(WorkCaseItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateItemForWorkCase([FromRoute] Guid workCaseItemId, [FromBody] UpdateWorkCaseItemCommand command, CancellationToken ct)
        {
            if (workCaseItemId != command.WorkCaseItemId)
                return BadRequest("Mismatched WorkCaseItem identifier between URL and body.");

            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpDelete("{workCaseItemId:guid}")]
        [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteItemForWorkCase([FromRoute] Guid workCaseItemId, CancellationToken ct)
        {
            var result = await Mediator.Send(new DeleteWorkCaseItemCommand(workCaseItemId), ct);
            return HandleResult(result);
        }

        #endregion
    }
}