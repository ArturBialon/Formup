using Application.Controllers.Base;
using Domain.DTO;
using Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    //[Authorize]
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
            var response = await _repository.AddProvider(provider);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProvider(ServiceProviderDTO provider)
        {
            var response = await _repository.EditProvider(provider);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            var response = await _repository.DeleteProviderById(id);
            return Ok(response);
        }
    }
}
