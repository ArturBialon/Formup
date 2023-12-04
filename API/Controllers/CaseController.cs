using Application.Controllers.Base;
using Application.Services.Interfaces;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [Authorize]
    public class CaseController : ApiControllerBase
    {
        public readonly ICaseDbRepository _repository;
        public CaseController(ICaseDbRepository clientRep)
        {
            _repository = clientRep;
        }

        [HttpGet]
        public async Task<IActionResult> GetCases()
        {

            ICollection<CaseListResponseDTO> cases = await _repository.GetCases();

            if (cases != null)
                return Ok(cases);
            else
                return NotFound("No data");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCaseById(int id)
        {

            CaseResponseDTO transportCase = await _repository.GetCaseById(id);

            if (transportCase != null)
                return Ok(transportCase);
            else
                return NotFound("No records matching to given id");
        }

        [HttpPost]
        public async Task<IActionResult> AddCase(CaseRequestDTO transportCase)
        {

            var result = await _repository.AddCase(transportCase);

            if (result == CommonEnum.CHANGES_SAVED)
            {
                return Ok("case successfully added");
            }
            else
            {
                return BadRequest("Error: " + result.ToString());
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCase(int id, CaseRequestDTO transportCase)
        {
            var result = await _repository.EditCase(id, transportCase);

            if (result == CommonEnum.CHANGES_SAVED)
            {
                return Ok("case successfully updated");
            }

            else if (result == CommonEnum.CANNOT_FIND)
            {
                return BadRequest("could not find case");
            }
            else
            {
                return BadRequest("Error: " + result.ToString());
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCase(int id)
        {
            var result = await _repository.DeleteCaseById(id);

            if (result == CommonEnum.SUCCESSFULLY_REMOVED)
            {
                return Ok("case successfully removed");
            }
            else if (result == CommonEnum.CANNOT_FIND)
            {
                return BadRequest("could not find case");
            }
            else
            {
                return BadRequest("Error: " + result.ToString());
            }
        }
    }
}
