using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Queries
{
    public record GetWorkCasesQuery(
        int PageNumber = 1,
        int PageSize = 50,
        string? Relation = null,
        string? ForwarderName = null,
        string? ClientName = null,
        string? Name = null,
        bool? IsAbandoned = null
    ) : IRequest<AppResult<PagedResult<WorkCaseResponse>>>;

    public class GetWorkCasesQueryHandler(FormupContext context)
        : IRequestHandler<GetWorkCasesQuery, AppResult<PagedResult<WorkCaseResponse>>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<PagedResult<WorkCaseResponse>>> Handle(GetWorkCasesQuery request, CancellationToken ct)
        {
            var optimalPageSize = Math.Clamp(request.PageSize, 1, 1000);
            var query = _context.WorkCases.AsNoTracking().AsQueryable();

            if (request.IsAbandoned != null)
                query = query.Where(x => x.IsAbandoned == request.IsAbandoned);

            if (!string.IsNullOrWhiteSpace(request.Relation))
                query = query.Where(x => x.Relation == request.Relation);

            if (!string.IsNullOrWhiteSpace(request.ForwarderName))
                query = query.Where(x => x.Forwarder.Name.Contains(request.ForwarderName) || x.Forwarder.Prefix.Contains(request.ForwarderName));

            if (!string.IsNullOrWhiteSpace(request.ClientName))
                query = query.Where(x => x.Client.Name.Contains(request.ClientName));

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(x => x.Name.Contains(request.Name));

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(x => x.CreatedAtUtc)
                .Skip((request.PageNumber - 1) * optimalPageSize)
                .Take(optimalPageSize)
                .Select(x => new WorkCaseResponse
                {
                    Id = x.Id.Value,
                    Name = x.Name,
                    Amount = x.Amount,
                    Currency = x.CurrencyCode,
                    Relation = x.Relation,
                    ForwarderName = x.Forwarder.FullName,
                    ClientName = x.Client.Name,
                    ClientId = x.Client.Id.Value,
                    ForwarderId = x.Forwarder.Id.Value,
                    IsAbandoned = x.IsAbandoned,
                })
                .ToListAsync(ct);

            var pagedResult = new PagedResult<WorkCaseResponse>(items, totalCount, request.PageNumber, optimalPageSize);

            return AppResult<PagedResult<WorkCaseResponse>>.Success(pagedResult);
        }
    }
}