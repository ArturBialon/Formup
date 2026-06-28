using Application.Common.Results;
using Application.DTOs.Response;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Access;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Clients.Commands
{
    public record CreateClientCommand(
        string Tax,
        string Name,
        string Street,
        string Zip,
        string Coutry,
        decimal Credit,
        string Currency = "PLN"
    ) : IRequest<IAppResult<ClientResponse>>;

    public class CreateClientCommandHandler(FormupContext context, ICurrentUserService currentUserService)
        : IRequestHandler<CreateClientCommand, IAppResult<ClientResponse>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<IAppResult<ClientResponse>> Handle(CreateClientCommand request, CancellationToken ct)
        {
            var taxExists = await _context.Clients
                .AnyAsync(x => x.Tax == request.Tax.Trim(), ct);

            if (taxExists)
            {
                return AppResult<ClientResponse>.Failure("CLIENT.TAX_ALREADY_EXISTS");
            }

            decimal credit = 0;

            if (_currentUserService.Role == UserRole.Verifier.ToString())
                credit = request.Credit;

            var client = new Client
            {
                Tax = request.Tax.Trim(),
                Name = request.Name.Trim(),
                Street = request.Street.Trim(),
                Zip = request.Zip.Trim(),
                Coutry = request.Coutry.Trim(),
                Credit = credit,
                Currency = request.Currency.Trim()
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync(ct);

            var responseDto = new ClientResponse
            {
                Id = client.Id.Value,
                Tax = client.Tax,
                Name = client.Name,
                Street = client.Street,
                Zip = client.Zip,
                Coutry = client.Coutry,
                Credit = client.Credit,
                Currency = client.Currency
            };

            return AppResult<ClientResponse>.Success(responseDto);
        }
    }
}
