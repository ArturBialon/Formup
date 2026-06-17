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
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkCaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWorkCaseById(Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetWorkCaseByIdQuery(id), ct);

            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(WorkCaseResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddWorkCase([FromBody] CreateWorkCaseCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetWorkCaseById), new { id = result.Id }, result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(WorkCaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditWorkCase([FromBody] UpdateWorkCaseCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);

            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPatch("abandon/{id}")]
        [ProducesResponseType(typeof(WorkCaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AbandonWorkCase(Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new AbandonWorkCaseCommand(id), ct);
            return Ok(result);
        }
    }
}