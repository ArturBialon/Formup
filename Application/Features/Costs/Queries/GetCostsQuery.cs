using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Costs.Queries
{
    public record GetCostsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? SearchTerm = null,
        string? Currency = null,
        Guid? ServiceContractorId = null,
        DateTime? DateFrom = null,
        DateTime? DateTo = null
    ) : IRequest<IAppResult<PagedResult<CostDetailResponse>>>;

    public class GetCostsQueryHandler(FormupContext context) : IRequestHandler<GetCostsQuery, IAppResult<PagedResult<CostDetailResponse>>>
    {
        private readonly FormupContext _context = context;

        public async Task<IAppResult<PagedResult<CostDetailResponse>>> Handle(GetCostsQuery request, CancellationToken ct)
        {
            var query = _context.Costs.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();
                query = query.Where(x => x.Name.Contains(term, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(request.Currency))
            {
                var currency = request.Currency.Trim().ToUpper();
                query = query.Where(x => x.Currency == currency);
            }

            if (request.ServiceContractorId.HasValue)
            {
                query = query.Where(x => x.ServiceContractor.Id.Value == request.ServiceContractorId.Value);
            }

            if (request.DateFrom.HasValue)
            {
                query = query.Where(x => x.ServiceDate >= request.DateFrom.Value.Date);
            }
            if (request.DateTo.HasValue)
            {
                query = query.Where(x => x.ServiceDate <= request.DateTo.Value.Date);
            }

            int totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(x => x.ServiceDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new CostDetailResponse
                {
                    Id = c.Id.Value,
                    Name = c.Name,
                    Amount = c.Amount,
                    Currency = c.Currency,
                    Tax = c.Tax,
                    IssueDate = c.IssueDate,
                    ServiceDate = c.ServiceDate,
                    DocumentUrl = c.DocumentUrl,
                    WorkCaseItemId = c.WorkCaseItem.Id,
                    ServiceContractorId = c.ServiceContractor.Id
                })
                .ToListAsync(ct);

            var pagedResult = new PagedResult<CostDetailResponse>(items, totalCount, request.PageNumber, request.PageSize);

            return AppResult<PagedResult<CostDetailResponse>>.Success(pagedResult);
        }
    }
}
