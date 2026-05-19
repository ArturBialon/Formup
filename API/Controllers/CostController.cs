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

        [HttpGet("{id}")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCost(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
