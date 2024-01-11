using Application.Controllers.Base;
using Domain.Interfaces.Repository;
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
        public async Task<ICollection<CaseListResponseDTO>> GetCases()
        {

            ICollection<CaseListResponseDTO> cases = await _repository.GetAllCases();
            return cases;
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

            var response = await _repository.AddCase(transportCase);

            if (response == CommonEnum.CHANGES_SAVED)
            {
                return Ok("case successfully added");
            }
            else
            {
                return BadRequest("Error: " + response.ToString());
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCase(int id, CaseRequestDTO transportCase)
        {
            var response = await _repository.EditCase(id, transportCase);

            if (response == CommonEnum.CHANGES_SAVED)
            {
                return Ok("case successfully updated");
            }

            else if (response == CommonEnum.CANNOT_FIND)
            {
                return BadRequest("could not find case");
            }
            else
            {
                return BadRequest("Error: " + response.ToString());
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCase(int id)
        {
            var response = await _repository.DeleteCaseById(id);

            if (response == CommonEnum.SUCCESSFULLY_REMOVED)
            {
                return Ok("case successfully removed");
            }
            else if (response == CommonEnum.CANNOT_FIND)
            {
                return BadRequest("could not find case");
            }
            else
            {
                return BadRequest("Error: " + response.ToString());
            }
        }
    }
}
