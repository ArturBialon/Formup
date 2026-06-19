using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ServiceContractors.Queries
{
    public record GetServiceContractorByIdQuery(Guid Id) : IRequest<IAppResult<ServiceContractorResponse>>;

    // Modern Handler z użyciem Primary Constructor (C# 12)
    public class GetServiceContractorByIdQueryHandler(FormupContext context)
        : IRequestHandler<GetServiceContractorByIdQuery, IAppResult<ServiceContractorResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<IAppResult<ServiceContractorResponse>> Handle(
            GetServiceContractorByIdQuery request,
            CancellationToken ct)
        {
            var result = await _context.ServiceContractors
                .AsNoTracking()
                .Where(x => x.Id.Value == request.Id)
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
                })
                .FirstOrDefaultAsync(ct);

            if (result == null)
            {
                return AppResult<ServiceContractorResponse>.Failure("SERVICE_CONTRACTOR.NOT_FOUND");
            }

            return AppResult<ServiceContractorResponse>.Success(result);
        }
    }
}
