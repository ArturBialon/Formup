using API.Controllers.Base;
using Application.DTOs.Response;
using Application.Features.Invoices.Commands;
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
        [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpPut("{invoiceId:guid}")]
        [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateInvoice([FromRoute] Guid invoiceId, [FromBody] UpdateInvoiceCommand command, CancellationToken ct)
        {
            if (invoiceId != command.InvoiceId)
                return BadRequest("Mismatched Invoice identifier between URL and body.");

            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
