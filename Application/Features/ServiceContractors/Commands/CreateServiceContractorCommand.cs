using Application.Common.Results;
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
        bool IsActive
    ) : IRequest<AppResult<Unit>>;

    public class CreateServiceContractorHandler(FormupContext context)
        : IRequestHandler<CreateServiceContractorCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<Unit>> Handle(CreateServiceContractorCommand request, CancellationToken ct)
        {
            var taxExists = await _context.ServiceContractors.AnyAsync(x => x.Tax == request.Tax, ct);
            if (taxExists)
            {
                return AppResult<Unit>.Failure("CONTRACTOR.TAX.NOT_UNIQUE");
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
                IsActive = request.IsActive
            };

            var created = _context.ServiceContractors.Add(contractor);
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
