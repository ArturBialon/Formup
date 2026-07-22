using Application.Common.CurrencyServices;
using Application.Common.Results;
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
    ) : IRequest<AppResult<Unit>>;

    public class UpdateClientCommandHandler(FormupContext context, ICurrentUserService currentUserService, ICurrencyConverterService currencyConverterService)
        : IRequestHandler<UpdateClientCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ICurrencyConverterService _currencyConverterService = currencyConverterService;

        public async Task<AppResult<Unit>> Handle(UpdateClientCommand request, CancellationToken ct)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), ct);

            var taxExists = await _context.Clients
                .AnyAsync(x => x.Tax == request.Tax.Trim() && !x.Id.Equals(request.Id), ct);

            if (client == null)
                return AppResult<Unit>.Failure("CLIENT.NOT_FOUND");

            if (taxExists)
                return AppResult<Unit>.Failure("CLIENT.TAX_ALREADY_EXISTS");

            decimal calculatedCredit = client.Credit;

            if (_currentUserService.Role == UserRole.Verifier.ToString() || (request.Credit == client.Credit && request.CurrencyCode == client.CurrencyCode))
            {
                calculatedCredit = request.Credit;
                var calculationResult = await _currencyConverterService.ConvertToTargetCurrency(request.Credit, request.CurrencyCode!, "PLN", DateTime.UtcNow, ct);

                if (!calculationResult.IsSuccess)
                    return AppResult<Unit>.Failure(calculationResult.ErrorCode, calculationResult.ErrorData);

                calculatedCredit = calculationResult.Value;
                client.Credit = request.Credit;
                client.CreditInPln = calculatedCredit;
                client.CurrencyCode = request.CurrencyCode;
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
            client.IsActive = request.IsActive;

            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
