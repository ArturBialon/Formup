using Application.Common.Results;
using Application.DTOs.Response;
using Domain.Enums;
using Infrastructure.Access;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Clients.Commands
{
    public record UpdateClientCommand(
        Guid Id,
        string Tax,
        string Name,
        string Street,
        string Zip,
        string Coutry,
        decimal Credit,
        string Currency
    ) : IRequest<IAppResult<ClientResponse>>;
    public class UpdateClientCommandHandler(FormupContext context, ICurrentUserService currentUserService) : IRequestHandler<UpdateClientCommand, IAppResult<ClientResponse>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<IAppResult<ClientResponse>> Handle(UpdateClientCommand request, CancellationToken ct)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(x => x.Id.Value == request.Id, ct);

            var taxExists = await _context.Clients
                .AnyAsync(x => x.Tax == request.Tax.Trim() && x.Id.Value != request.Id, ct);

            if (client == null)
                return AppResult<ClientResponse>.Failure("CLIENT.NOT_FOUND");

            if (taxExists)
                return AppResult<ClientResponse>.Failure("CLIENT.TAX_ALREADY_EXISTS");

            decimal credit = client.Credit;

            if (_currentUserService.Role == UserRole.Verifier.ToString())
            {
                credit = request.Credit;
            }

            client.Tax = request.Tax.Trim();
            client.Name = request.Name.Trim();
            client.Street = request.Street.Trim();
            client.Zip = request.Zip.Trim();
            client.Coutry = request.Coutry.Trim();
            client.Credit = credit;
            client.Currency = request.Currency.Trim();

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
