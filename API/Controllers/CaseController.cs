using Application.Controllers.Base;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    //[Authorize]
    public class CaseController : ApiControllerBase
    {
        private readonly ICaseService _caseService;

        public CaseController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        [HttpGet]
        public async Task<ICollection<CaseListResponseDTO>> GetCases()
        {

            ICollection<CaseListResponseDTO> cases = await _caseService.GetCases();
            return cases;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCaseById(int id)
        {

            CaseResponseDTO transportCase = await _caseService.GetCaseById(id);

            if (transportCase != null)
                return Ok(transportCase);
            else
                return NotFound("No records matching to given id");
        }

        [HttpPost]
        public async Task<IActionResult> AddCase(CaseRequestDTO transportCase)
        {
            var response = await _caseService.CreateNewCase(transportCase);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditCase(CaseRequestDTO transportCase)
        {
            var response = await _caseService.EditCase(transportCase);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCase(int id)
        {
            var response = await _caseService.DeleteCaseById(id);
            return Ok(response);
        }
    }
}
