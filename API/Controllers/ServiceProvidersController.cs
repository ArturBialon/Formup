using Application.Controllers.Base;
using Domain.Interfaces.Repository;
using Domain.DTO;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [Authorize]
    public class ServiceProvidersController : ApiControllerBase
    {

        public readonly IServiceProviderDbRepository _repository;
        public ServiceProvidersController(IServiceProviderDbRepository proversRep)
        {
            _repository = proversRep;
        }

        [HttpGet]
        public async Task<IActionResult> GetProviders()
        {

            ICollection<ServiceProviderDTO> seriveProviders = await _repository.GetProviders();

            if (seriveProviders != null)
                return Ok(seriveProviders);
            else
                return NotFound("No data");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProviderById(int id)
        {

            ServiceProviderDTO provider = await _repository.GetProviderById(id);

            if (provider != null)
                return Ok(provider);
            else
                return NotFound("No records matching to given id");
        }

        [HttpPost]
        public async Task<IActionResult> AddProvider(ServiceProviderDTO provider)
        {

            var result = await _repository.AddProvider(provider);

            if (result == CommonEnum.SUCCESSFULLY_ADDED)
            {
                return Ok("provider successfully added");
            }
            else
                return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProvider(int id, ServiceProviderDTO provider)
        {
            var result = await _repository.EditProvider(id, provider);

            if (result == CommonEnum.CHANGES_SAVED)
            {
                return Ok("provider successfully edited");
            }
            else
                return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            var result = await _repository.DeleteProviderById(id);

            if (result == CommonEnum.SUCCESSFULLY_REMOVED)
            {
                return Ok("provider removed");
            }
            else
                return BadRequest(result);
        }
    }
}
