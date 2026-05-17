using Application.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    //[Authorize]
    public class WorkCaseController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetWorkCases()
        {
            throw new NotImplementedException();
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
