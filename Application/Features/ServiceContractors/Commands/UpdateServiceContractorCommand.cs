using Application.Common.Results;
using Application.DTOs.Response;
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
        string? PhoneNumber
    ) : IRequest<AppResult<ServiceContractorResponse>>;

    public class UpdateServiceContractorHandler(FormupContext context)
        : IRequestHandler<UpdateServiceContractorCommand, AppResult<ServiceContractorResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<ServiceContractorResponse>> Handle(UpdateServiceContractorCommand request, CancellationToken ct)
        {
            var contractor = await _context.ServiceContractors.FindAsync([request.Id], ct);
            if (contractor == null)
                return AppResult<ServiceContractorResponse>.Failure("CONTRACTOR.NOT_FOUND");

            if (contractor.Tax != request.Tax)
            {
                var taxExists = await _context.ServiceContractors
                    .AnyAsync(x => x.Tax == request.Tax && x.Id.Value != request.Id, ct);

                if (taxExists)
                {
                    return AppResult<ServiceContractorResponse>.Failure("CONTRACTOR.TAX.NOT_UNIQUE");
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

            await _context.SaveChangesAsync(ct);

            var result = new ServiceContractorResponse
            {
                Id = contractor.Id.Value,
                Name = contractor.Name,
                Tax = contractor.Tax,
                Country = contractor.Country,
                City = contractor.City,
                Zip = contractor.Zip,
                Street = contractor.Street,
                HouseNumber = contractor.HouseNumber,
                ApartmentNumber = contractor.ApartmentNumber,
                Email = contractor.Email,
                PhoneNumber = contractor.PhoneNumber
            };

            return AppResult<ServiceContractorResponse>.Success(result);
        }
    }
}
