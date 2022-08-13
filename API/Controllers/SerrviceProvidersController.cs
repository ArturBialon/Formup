using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Services;
using API.Services.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/serviceProviders")]
    public class SerrviceProvidersController : ControllerBase
    {

        public readonly IServiceProviderDbRepository _repository;
        public SerrviceProvidersController(IServiceProviderDbRepository proversRep)
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
                return NotFound("Brak rekorów w bazie");
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

            if (result == "provider successfully added")
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProvider(int id, ServiceProviderDTO provider)
        {
            var result = await _repository.EditProvider(id, provider);

            if (result == "provider successfully edited")
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            var result = await _repository.DeleteProviderById(id);

            if (result == "provider removed")
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
    }
}
