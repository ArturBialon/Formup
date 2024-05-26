using Application.Controllers.Base;
using Domain.DTO;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    //[Authorize]
    public class ServiceProvidersController : ApiControllerBase
    {

        public readonly IServiceProviderService _service;
        public ServiceProvidersController(IServiceProviderService proversService)
        {
            _service = proversService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProviders()
        {

            ICollection<ServiceProviderDTO> seriveProviders = await _service.GetProviders();

            if (seriveProviders != null)
                return Ok(seriveProviders);
            else
                return NotFound("No data");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProviderById(int id)
        {

            var provider = await _service.GetProviderById(id);

            if (provider != null)
                return Ok(provider);
            else
                return NotFound("No records matching to given id");
        }

        [HttpPost]
        public async Task<IActionResult> AddProvider(ServiceProviderDTO provider)
        {
            var response = await _service.AddProvider(provider);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProvider(ServiceProviderDTO provider)
        {
            var response = await _service.EditProvider(provider);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            var response = await _service.DeleteProvider(id);
            return Ok(response);
        }
    }
}
