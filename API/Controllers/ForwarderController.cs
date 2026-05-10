using Application.Controllers.Base;
using Domain.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Application.Controllers
{
    //[Authorize]
    public class ForwarderController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetForwarders()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetForwarderById(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditForwarder(ForwarderRequestDTO forwarder)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForwarder(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}
