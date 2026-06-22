using API.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    public class CostController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCosts()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCostById(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> AddCost()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditCost()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCost(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
