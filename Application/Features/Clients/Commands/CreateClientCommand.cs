using Application.Common.CurrencyServices;
using Application.Common.Results;
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
    ) : IRequest<AppResult<Unit>>;

    public class CreateClientCommandHandler(FormupContext context, ICurrentUserService currentUserService, ICurrencyConverterService currencyConverterService)
    : IRequestHandler<CreateClientCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ICurrencyConverterService _currencyConverterService = currencyConverterService;

        public async Task<AppResult<Unit>> Handle(CreateClientCommand request, CancellationToken ct)
        {
            var taxExists = await _context.Clients
                .AnyAsync(x => x.Tax == request.Tax.Trim(), ct);

            if (taxExists)
            {
                return AppResult<Unit>.Failure("CLIENT.TAX_ALREADY_EXISTS");
            }

            decimal assignableCredit = 0;
            decimal calculatedCredit = 0;

            if (_currentUserService.Role == UserRole.Verifier.ToString())
            {
                var calculationResult = await _currencyConverterService.ConvertToTargetCurrency(request.Credit, request.CurrencyCode ?? "PLN", "PLN", DateTime.UtcNow, ct);

                if (!calculationResult.IsSuccess)
                    return AppResult<Unit>.Failure(calculationResult.ErrorCode, calculationResult.ErrorData);

                calculatedCredit = calculationResult.Value;
                assignableCredit = request.Credit;
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
                Credit = assignableCredit,
                CreditInPln = calculatedCredit,
                CurrencyCode = request.CurrencyCode,
                IsActive = request.IsActive,
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync(ct);


            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
