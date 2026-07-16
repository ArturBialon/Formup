using Application.Common.Results;
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
        bool IsActive
    ) : IRequest<AppResult<Unit>>;

    public class UpdateServiceContractorHandler(FormupContext context)
        : IRequestHandler<UpdateServiceContractorCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<Unit>> Handle(UpdateServiceContractorCommand request, CancellationToken ct)
        {
            var contractor = await _context.ServiceContractors.FirstOrDefaultAsync(sc => sc.Id.Equals(request.Id), cancellationToken: ct);
            if (contractor == null)
                return AppResult<Unit>.Failure("CONTRACTOR.NOT_FOUND");

            if (contractor.Tax != request.Tax)
            {
                var taxExists = await _context.ServiceContractors
                    .AnyAsync(x => x.Tax == request.Tax && !x.Id.Equals(request.Id), ct);

                if (taxExists)
                {
                    return AppResult<Unit>.Failure("CONTRACTOR.TAX.NOT_UNIQUE");
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

            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
