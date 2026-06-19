using Application.Common.Results;
using Application.DTOs.Response;
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
        string? PhoneNumber
    ) : IRequest<AppResult<ServiceContractorResponse>>;

    public class CreateServiceContractorHandler(FormupContext context)
        : IRequestHandler<CreateServiceContractorCommand, AppResult<ServiceContractorResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<ServiceContractorResponse>> Handle(CreateServiceContractorCommand request, CancellationToken ct)
        {
            var taxExists = await _context.ServiceContractors.AnyAsync(x => x.Tax == request.Tax, ct);
            if (taxExists)
            {
                return AppResult<ServiceContractorResponse>.Failure("CONTRACTOR.VALIDATION.TAX.NOT_UNIQUE");
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
                PhoneNumber = request.PhoneNumber
            };

            var created = _context.ServiceContractors.Add(contractor);
            await _context.SaveChangesAsync(ct);

            var response = new ServiceContractorResponse
            {
                Id = created.Entity.Id.Value,
                Name = created.Entity.Name,
                Tax = created.Entity.Tax,
                Country = created.Entity.Country,
                City = created.Entity.City,
                Street = created.Entity.Street,
                Zip = created.Entity.Zip,
                HouseNumber = created.Entity.HouseNumber,
                ApartmentNumber = created.Entity.ApartmentNumber,
                Email = created.Entity.Email,
                PhoneNumber = created.Entity.PhoneNumber
            };

            return AppResult<ServiceContractorResponse>.Success(response);
        }
    }
}
