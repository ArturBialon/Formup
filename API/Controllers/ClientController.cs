using API.Controllers.Base;
using Application.Common.Results;
using Application.DTOs.Response;
using Application.Features.Clients.Commands;
using Application.Features.Clients.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ClientController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ClientListItemResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClients([FromQuery] GetClientsQuery query, CancellationToken ct)
        {
            var result = await Mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ClientDetailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClientById([FromQuery] Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetClientByIdQuery(id), ct);
            return HandleResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return HandleResult(result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteClient([FromQuery] Guid id, CancellationToken ct)
        {
            var result = await Mediator.Send(new DeleteClientCommand(id), ct);
            return HandleResult(result);
        }
    }
}
