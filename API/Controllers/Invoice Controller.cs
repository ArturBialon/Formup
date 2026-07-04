using API.Controllers.Base;
using Application.Common.Results;
using Application.DTOs.Response;
using Application.Features.Invoices.Commands;
using Application.Features.Invoices.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class InvoiceController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<InvoiceResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInvoices([FromQuery] GetInvoicesQuery query, CancellationToken ct)
        {
            var result = await Mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(InvoiceDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInvoiceById([FromQuery] Guid invoiceId, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetInvoiceByIdQuery(invoiceId), ct);
            return HandleResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateInvoice([FromQuery] Guid invoiceId, [FromBody] UpdateInvoiceCommand command, CancellationToken ct)
        {
            if (invoiceId != command.InvoiceId)
                return BadRequest("REQUEST.ID_MISSMATCH");

            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteInvoice([FromQuery] Guid invoiceId)
        {
            var result = await Mediator.Send(new DeleteInvoiceCommand(invoiceId));
            return HandleResult(result);
        }
    }
}
