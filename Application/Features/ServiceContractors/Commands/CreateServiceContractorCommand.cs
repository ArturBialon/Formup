using Application.Common.Results;
using Application.DTOs.Request;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ServiceContractors.Commands
{
    public record CreateServiceContractorCommand(
        string Name,
        string Tax,
        string Country,
        string City,
        string Zip,
        string Street,
        string HouseNumber,
        string? ApartmentNumber,
        string? Email,
        string? PhoneNumber,
        bool IsActive,
        List<BankAccountRequest>? BankAccounts = null
    ) : IRequest<AppResult<Guid>>;

    public class CreateServiceContractorHandler(FormupContext context)
        : IRequestHandler<CreateServiceContractorCommand, AppResult<Guid>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<Guid>> Handle(CreateServiceContractorCommand request, CancellationToken ct)
        {
            var taxExists = await _context.ServiceContractors.AnyAsync(x => x.Tax == request.Tax, ct);
            if (taxExists) return AppResult<Guid>.Failure("CONTRACTOR.TAX.NOT_UNIQUE");

            if (request.BankAccounts != null)
            {
                var ibansToCheck = request.BankAccounts
                .Select(x => x.IBAN)
                .Where(iban => !string.IsNullOrWhiteSpace(iban))
                .Distinct()
                .ToList();

                if (ibansToCheck.Count != 0)
                {
                    bool accountExists = await _context.BankAccounts
                        .AnyAsync(x => ibansToCheck.Contains(x.IBAN), ct);

                    if (accountExists)
                    {
                        return AppResult<Guid>.Failure("CONTRACTOR.BANK_ACCOUNT_NOT_UNIQUE");
                    }
                }
            }

            var mainAccounts = request.BankAccounts?.Where(x => x.IsMain).Skip(1);

            if (mainAccounts != null)
            {
                foreach (var account in mainAccounts)
                {
                    account.IsMain = false;
                }
            }

            var contractor = new ServiceContractor
            {
                Name = request.Name,
                Tax = request.Tax,
                Country = request.Country,
                City = request.City,
                Zip = request.Zip,
                Street = request.Street,
                HouseNumber = request.HouseNumber,
                ApartmentNumber = request.ApartmentNumber,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.IsActive,
                BankAccounts = request.BankAccounts?.Select(b => new BankAccount
                {
                    BankName = b.BankName,
                    IBAN = b.IBAN,
                    CurrencyCode = b.CurrencyCode,
                    IsMain = b.IsMain
                }).ToList() ?? []
            };

            var created = _context.ServiceContractors.Add(contractor);
            await _context.SaveChangesAsync(ct);

            return AppResult<Guid>.Success(created.Entity.Id);
        }
    }
}