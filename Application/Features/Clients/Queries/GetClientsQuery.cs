using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Clients.Queries
{
    public record GetClientsQuery(
        string? Tax = null,
        string? Name = null,
        string? Street = null,
        string? Zip = null,
        string? Country = null,
        int PageNumber = 1,
        int PageSize = 50,
        bool? IsActive = null
    ) : IRequest<IAppResult<PagedResult<ClientListItemResponse>>>;

    public class GetClientsQueryHandler(FormupContext context)
    : IRequestHandler<GetClientsQuery, IAppResult<PagedResult<ClientListItemResponse>>>
    {
        private readonly FormupContext _context = context;

        public async Task<IAppResult<PagedResult<ClientListItemResponse>>> Handle(GetClientsQuery request, CancellationToken ct)
        {
            var query = _context.Clients.AsNoTracking().AsQueryable();

            if (request.IsActive != null)
                query = query.Where(x => x.IsActive == request.IsActive);

            if (!string.IsNullOrWhiteSpace(request.Tax))
                query = query.Where(x => x.Tax.Contains(request.Tax.Trim()));

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(x => x.Name.Contains(request.Name.Trim()));

            if (!string.IsNullOrWhiteSpace(request.Street))
                query = query.Where(x => x.Street.Contains(request.Street.Trim()));

            if (!string.IsNullOrWhiteSpace(request.Zip))
                query = query.Where(x => x.Zip.Contains(request.Zip.Trim()));

            if (!string.IsNullOrWhiteSpace(request.Country))
                query = query.Where(x => x.Country.Contains(request.Country.Trim()));


            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(client => new ClientListItemResponse
                {
                    Id = client.Id.Value,
                    Tax = client.Tax,
                    Name = client.Name,
                    Country = client.Country,
                    City = client.City,
                    Zip = client.Zip,
                    Street = client.Street,
                    HouseNumber = client.HouseNumber,
                    ApartmentNumber = client.ApartmentNumber,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,
                    Credit = client.Credit,
                    Currency = client.Currency,
                    InvoicesCount = client.Invoices.Count(),
                    WorkCasesCount = client.WorkCases.Count(),
                    IsActive = client.IsActive
                })
                .ToListAsync(ct);
            var pagedResult = new PagedResult<ClientListItemResponse>(items, totalCount, request.PageNumber, request.PageSize);

            return AppResult<PagedResult<ClientListItemResponse>>.Success(pagedResult);
        }
    }
}
