using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.DTO.Request;
using API.DTO.Response;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/Cases")]
    public class CaseController : ControllerBase
    {
        public readonly ICaseDbRepository _repository;
        public CaseController(ICaseDbRepository clientRep)
        {
            _repository = clientRep;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCases()
        {

            ICollection<CaseListResponseDTO> cases = await _repository.GetCases();

            if (cases != null)
                return Ok(cases);
            else
                return NotFound("No data");
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCaseById(int id)
        {

            CaseResponseDTO @case = await _repository.GetCaseById(id);

            if (@case != null)
                return Ok(@case);
            else
                return NotFound("No records matching to given id");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCase(CaseRequestDTO @case)
        {

            var result = await _repository.AddCase(@case);

            if (result == Enum.CommonEnum.CHANGES_SAVED)
            {
                return Ok("case successfully added");
            }
            else
            {
                return BadRequest("Error: " + result.ToString());
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCase(int id, CaseRequestDTO @case)
        {
            var result = await _repository.EditCase(id, @case);

            if (result == Enum.CommonEnum.CHANGES_SAVED)
            {
                return Ok("case successfully updated");
            }

            else if (result == Enum.CommonEnum.CANNOT_FIND)
            {
                return BadRequest("could not find case");
            }
            else
            {
                return BadRequest("Error: " + result.ToString());
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCase(int id)
        {
            var result = await _repository.DeleteCaseById(id);

            if (result == Enum.CommonEnum.SUCCESSFULLY_REMOVED)
            {
                return Ok("case successfully removed");
            }
            else if (result == Enum.CommonEnum.CANNOT_FIND)
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
