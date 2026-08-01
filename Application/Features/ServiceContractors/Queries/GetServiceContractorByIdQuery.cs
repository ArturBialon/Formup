using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ServiceContractors.Queries
{
    public record GetServiceContractorByIdQuery(Guid Id) : IRequest<AppResult<ServiceContractorResponse>>;

    public class GetServiceContractorByIdQueryHandler(FormupContext context)
        : IRequestHandler<GetServiceContractorByIdQuery, AppResult<ServiceContractorResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<ServiceContractorResponse>> Handle(
            GetServiceContractorByIdQuery request,
            CancellationToken ct)
        {
            var result = await _context.ServiceContractors
                .AsNoTracking()
                .Where(x => x.Id.Equals(request.Id))
                .Select(x => new ServiceContractorResponse
                {
                    Id = x.Id,
                    Tax = x.Tax,
                    Name = x.Name,
                    Country = x.Country,
                    City = x.City,
                    Zip = x.Zip,
                    Street = x.Street,
                    HouseNumber = x.HouseNumber,
                    ApartmentNumber = x.ApartmentNumber,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    IsActive = x.IsActive,
                    BankAccounts = x.BankAccounts.Select(b => new BankAccountResponse
                    {
                        Id = b.Id,
                        IBAN = b.IBAN,
                        BankName = b.BankName,
                        CurrencyCode = b.CurrencyCode,
                        IsMain = b.IsMain
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (result == null)
            {
                return AppResult<ServiceContractorResponse>.Failure("CONTRACTOR.NOT_FOUND");
            }

            return AppResult<ServiceContractorResponse>.Success(result);
        }
    }
}
