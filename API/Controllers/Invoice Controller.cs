using API.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    public class InvoiceController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetInvoice()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetInvoiceById(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> AddInvoiceCase()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> EditInvoiceCase()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoiceCase(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
