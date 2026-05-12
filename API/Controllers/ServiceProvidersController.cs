using Application.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Application.Controllers
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
        public async Task<IActionResult> GetProviderById(Guid id)
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
        public async Task<IActionResult> DeleteProvider(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
