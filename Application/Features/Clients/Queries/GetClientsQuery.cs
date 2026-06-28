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
        string? Coutry = null,
        int PageNumber = 1,
        int PageSize = 50
    ) : IRequest<IAppResult<PagedResult<ClientListItemResponse>>>;

    public class GetClientsQueryHandler(FormupContext context)
        : IRequestHandler<GetClientsQuery, IAppResult<PagedResult<ClientListItemResponse>>>
    {
        private readonly FormupContext _context = context;

        public async Task<IAppResult<PagedResult<ClientListItemResponse>>> Handle(GetClientsQuery request, CancellationToken ct)
        {
            var query = _context.Clients.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Tax))
                query = query.Where(x => x.Tax.Contains(request.Tax.Trim(), StringComparison.CurrentCultureIgnoreCase));


            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(x => x.Name.Contains(request.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));


            if (!string.IsNullOrWhiteSpace(request.Street))
                query = query.Where(x => x.Street.Contains(request.Street.Trim(), StringComparison.CurrentCultureIgnoreCase));


            if (!string.IsNullOrWhiteSpace(request.Zip))
                query = query.Where(x => x.Zip.Contains(request.Zip.Trim(), StringComparison.CurrentCultureIgnoreCase));


            if (!string.IsNullOrWhiteSpace(request.Coutry))
                query = query.Where(x => x.Coutry.Contains(request.Coutry.Trim(), StringComparison.CurrentCultureIgnoreCase));


            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(client => new ClientListItemResponse
                {
                    Id = client.Id.Value,
                    Tax = client.Tax,
                    Name = client.Name,
                    Street = client.Street,
                    Zip = client.Zip,
                    Coutry = client.Coutry,
                    Credit = client.Credit,
                    Currency = client.Currency,
                    InvoicesCount = client.Invoices.Count(),
                    WorkCasesCount = client.WorkCases.Count()
                })
                .ToListAsync(ct);

            var pagedResult = new PagedResult<ClientListItemResponse>(items, totalCount, request.PageNumber, request.PageSize);

            return AppResult<PagedResult<ClientListItemResponse>>.Success(pagedResult);
        }
    }
}
