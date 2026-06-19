using API.Controllers.Base;
using Application.Common.Models;
using Application.DTOs.Response;
using Application.Features.WorkCases.Commands;
using Application.Features.WorkCases.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    public class WorkCaseController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<WorkCaseList>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkCases([FromQuery] GetWorkCasesQuery query, CancellationToken ct)
        {
            var result = await Mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkCaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkCaseById(Guid id, CancellationToken ct)
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

        [HttpPatch("abandon/{id}")]
        [ProducesResponseType(typeof(WorkCaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AbandonWorkCase(Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new AbandonWorkCaseCommand(id), ct);
            return HandleResult(result);
        }
    }
}