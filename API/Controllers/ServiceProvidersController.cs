using API.Controllers.Base;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    public class ServiceProvidersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProviders()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProviderById(ServiceContractor.EntityId id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> AddProvider()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditProvider()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(ServiceContractor.EntityId id)
        {
            throw new NotImplementedException();
        }
    }
}
