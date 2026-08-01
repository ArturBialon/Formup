using Application.Common.Results;
using Application.DTOs.Request;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ServiceContractors.Commands
{
    public record UpdateServiceContractorCommand(
        Guid Id,
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

    public class UpdateServiceContractorHandler(FormupContext context)
        : IRequestHandler<UpdateServiceContractorCommand, AppResult<Guid>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<Guid>> Handle(UpdateServiceContractorCommand request, CancellationToken ct)
        {
            var contractor = await _context.ServiceContractors
                .Include(x => x.BankAccounts)
                .FirstOrDefaultAsync(sc => sc.Id.Equals(request.Id), cancellationToken: ct);

            if (contractor == null)
                return AppResult<Guid>.Failure("CONTRACTOR.NOT_FOUND");

            if (contractor.Tax != request.Tax)
            {
                var taxExists = await _context.ServiceContractors
                    .AnyAsync(x => x.Tax == request.Tax && !x.Id.Equals(request.Id), ct);

                if (taxExists)
                {
                    return AppResult<Guid>.Failure("CONTRACTOR.TAX.NOT_UNIQUE");
                }
            }

            contractor.Name = request.Name;
            contractor.Tax = request.Tax;
            contractor.Country = request.Country;
            contractor.City = request.City;
            contractor.Zip = request.Zip;
            contractor.Street = request.Street;
            contractor.HouseNumber = request.HouseNumber;
            contractor.ApartmentNumber = request.ApartmentNumber;
            contractor.Email = request.Email;
            contractor.PhoneNumber = request.PhoneNumber;
            contractor.IsActive = request.IsActive;

            var requestedAccounts = request.BankAccounts ?? [];
            var existingAccounts = contractor.BankAccounts.ToList();

            var requestedIds = requestedAccounts
                .Where(b => b.Id.HasValue && !b.Id.Equals(Guid.Empty))
                .Select(b => b.Id!.Value)
                .ToList();

            var accountsToRemove = existingAccounts
                .Where(b => !requestedIds.Any(reqId => reqId.Equals(b.Id)))
                .ToList();

            foreach (var accountToRemove in accountsToRemove)
            {
                _context.BankAccounts.Remove(accountToRemove);
            }

            foreach (var accountReq in requestedAccounts)
            {
                if (accountReq.Id.HasValue && !accountReq.Id.Equals(Guid.Empty))
                {
                    var existingAccount = existingAccounts.FirstOrDefault(b => b.Id.Equals(accountReq.Id.Value));
                    if (existingAccount != null)
                    {
                        existingAccount.BankName = accountReq.BankName;
                        existingAccount.IBAN = accountReq.IBAN;
                        existingAccount.CurrencyCode = accountReq.CurrencyCode;
                        existingAccount.IsMain = accountReq.IsMain;
                    }
                }
                else
                {
                    contractor.BankAccounts.Add(new BankAccount
                    {
                        BankName = accountReq.BankName,
                        IBAN = accountReq.IBAN,
                        CurrencyCode = accountReq.CurrencyCode,
                        IsMain = accountReq.IsMain
                    });
                }
            }

            await _context.SaveChangesAsync(ct);

            return AppResult<Guid>.Success(contractor.Id);
        }
    }
}