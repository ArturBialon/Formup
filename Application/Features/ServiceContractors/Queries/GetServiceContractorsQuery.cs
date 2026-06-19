using Application.Common.Models;
using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ServiceContractors.Queries
{
    public record GetServiceContractorsQuery(
        int PageNumber = 1,
        int PageSize = 50,
        string? Name = null,
        string? Tax = null,
        string? City = null,
        string? Country = null
    ) : IRequest<IAppResult<PagedResult<ServiceContractorResponse>>>;

    public class GetServiceContractorsQueryHandler(FormupContext context)
        : IRequestHandler<GetServiceContractorsQuery, IAppResult<PagedResult<ServiceContractorResponse>>>
    {
        private readonly FormupContext _context = context;

        public async Task<IAppResult<PagedResult<ServiceContractorResponse>>> Handle(GetServiceContractorsQuery request, CancellationToken ct)
        {
            var query = _context.ServiceContractors.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(x => x.Name.Contains(request.Name));

            if (!string.IsNullOrWhiteSpace(request.Tax))
                query = query.Where(x => x.Tax.Contains(request.Tax));

            if (!string.IsNullOrWhiteSpace(request.City))
                query = query.Where(x => x.City.Contains(request.City));

            if (!string.IsNullOrWhiteSpace(request.Country))
                query = query.Where(x => x.Country == request.Country);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(x => x.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
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
                .ToListAsync(ct);

            var pagedResult = new PagedResult<ServiceContractorResponse>(items, totalCount, request.PageNumber, request.PageSize);

            return AppResult<PagedResult<ServiceContractorResponse>>.Success(pagedResult);
        }
    }
}
