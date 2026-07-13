using Application.Common.CurrencyServices;
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
        string? CurrencyCode = "PLN"
    ) : IRequest<IAppResult<ClientResponse>>;

    public class CreateClientCommandHandler(FormupContext context, ICurrentUserService currentUserService, ICurrencyConverterService currencyConverterService)
    : IRequestHandler<CreateClientCommand, IAppResult<ClientResponse>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ICurrencyConverterService _currencyConverterService = currencyConverterService;

        public async Task<IAppResult<ClientResponse>> Handle(CreateClientCommand request, CancellationToken ct)
        {
            var taxExists = await _context.Clients
                .AnyAsync(x => x.Tax == request.Tax.Trim(), ct);

            if (taxExists)
            {
                return AppResult<ClientResponse>.Failure("CLIENT.TAX_ALREADY_EXISTS");
            }

            decimal calculatedCredit = 0;

            if (_currentUserService.Role == UserRole.Verifier.ToString())
            {
                calculatedCredit = request.Credit;

                if (request.CurrencyCode != null && request.CurrencyCode != "PLN")
                {
                    var calculationResult = await _currencyConverterService.ConvertToTargetCurrency(request.Credit, request.CurrencyCode, "PLN", DateTime.Now, ct);

                    if (!calculationResult.IsSuccess)
                        return AppResult<ClientResponse>.Failure(calculationResult.ErrorCode, calculationResult.ErrorData);

                    calculatedCredit = calculationResult.Value;
                }
            }

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
                Credit = calculatedCredit,
                Currency = "PLN",
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
                Credit = calculatedCredit,
                Currency = "PLN",
                IsActive = client.IsActive
            };

            return AppResult<ClientResponse>.Success(responseDto);
        }
    }
}
