using Application.Controllers.Base;
using Domain.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> AddWorkCase(WorkCaseRequestDTO transportWorkCase)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditWorkCase(WorkCaseRequestDTO transportWorkCase)
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
