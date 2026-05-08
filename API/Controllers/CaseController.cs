using Application.Controllers.Base;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    //[Authorize]
    public class WorkCaseController : ApiControllerBase
    {
        private readonly IWorkCaseService _caseService;

        public WorkCaseController(IWorkCaseService caseService)
        {
            _caseService = caseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkCases()
        {

            ICollection<WorkCaseListResponseDTO> cases = await _caseService.GetWorkCases();
            return Ok(cases);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkCaseById(Guid id)
        {

            WorkCaseResponseDTO transportWorkCase = await _caseService.GetWorkCaseById(id);

            if (transportWorkCase != null)
                return Ok(transportWorkCase);
            else
                return NotFound("No records matching to given id");
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkCase(WorkCaseRequestDTO transportWorkCase)
        {
            var response = await _caseService.CreateNewWorkCase(transportWorkCase);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditWorkCase(WorkCaseRequestDTO transportWorkCase)
        {
            var response = await _caseService.EditWorkCase(transportWorkCase);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkCase(Guid id)
        {
            var response = await _caseService.DeleteWorkCaseById(id);
            return Ok(response);
        }
    }
}
