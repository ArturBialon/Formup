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
        string City,
        string HouseNumber,
        string Zip,
        string Country,
        decimal Credit,
        bool IsActive,
        string? ApartmentNumber = null,
        string? Email = null,
        string? PhoneNumber = null,
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
                Country = request.Country.Trim(),
                City = request.City.Trim(),
                Zip = request.Zip.Trim(),
                Street = request.Street.Trim(),
                HouseNumber = request.HouseNumber.Trim(),
                ApartmentNumber = request.ApartmentNumber?.Trim(),
                Email = request.Email?.Trim(),
                PhoneNumber = request.PhoneNumber?.Trim(),
                Credit = credit,
                Currency = request.Currency.Trim(),
                IsActive = request.IsActive,
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync(ct);

            var responseDto = new ClientResponse
            {
                Id = client.Id.Value,
                Tax = client.Tax,
                Name = client.Name,
                Country = client.Country,
                City = client.City,
                Zip = client.Zip,
                Street = client.Street,
                HouseNumber = client.HouseNumber,
                ApartmentNumber = client.ApartmentNumber,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Credit = client.Credit,
                Currency = client.Currency,
                IsActive = client.IsActive
            };

            return AppResult<ClientResponse>.Success(responseDto);
        }
    }
}
