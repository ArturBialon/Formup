using API.Controllers.Base;
using Application.Common.Models;
using Application.DTOs.Response;
using Application.Features.WorkCases.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    public class WorkCaseController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<WorkCaseListDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkCases([FromQuery] GetWorkCasesQuery query, CancellationToken ct)
        {
            var result = await Mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkCaseById(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkCase()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditWorkCase()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkCase(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
