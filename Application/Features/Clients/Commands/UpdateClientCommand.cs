using Application.Common.CurrencyServices;
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

    public class UpdateClientCommandHandler(FormupContext context, ICurrentUserService currentUserService, ICurrencyConverterService currencyConverterService)
        : IRequestHandler<UpdateClientCommand, IAppResult<ClientResponse>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ICurrencyConverterService _currencyConverterService = currencyConverterService;

        public async Task<IAppResult<ClientResponse>> Handle(UpdateClientCommand request, CancellationToken ct)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), ct);

            var taxExists = await _context.Clients
                .AnyAsync(x => x.Tax == request.Tax.Trim() && !x.Id.Equals(request.Id), ct);

            if (client == null)
                return AppResult<ClientResponse>.Failure("CLIENT.NOT_FOUND");

            if (taxExists)
                return AppResult<ClientResponse>.Failure("CLIENT.TAX_ALREADY_EXISTS");

            decimal calculatedCredit = client.Credit;

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

            client.Tax = request.Tax.Trim();
            client.Name = request.Name.Trim();
            client.Street = request.Street.Trim();
            client.City = request.City.Trim();
            client.Zip = request.Zip.Trim();
            client.Country = request.Country.Trim();
            client.HouseNumber = request.HouseNumber.Trim();
            client.ApartmentNumber = request.ApartmentNumber?.Trim();
            client.Email = request.Email?.Trim();
            client.PhoneNumber = request.PhoneNumber?.Trim();
            client.Credit = calculatedCredit;
            client.Currency = "PLN";
            client.IsActive = request.IsActive;

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
                IsActive = client.IsActive,
            };

            return AppResult<ClientResponse>.Success(responseDto);
        }
    }
}
