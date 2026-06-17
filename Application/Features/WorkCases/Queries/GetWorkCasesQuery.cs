using Application.Common.Models;
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
        Guid? ForwarderId = null,
        Guid? ClientId = null,
        string? SearchPhrase = null
    ) : IRequest<PagedResult<WorkCaseList>>;

    public class GetWorkCasesHandler : IRequestHandler<GetWorkCasesQuery, PagedResult<WorkCaseList>>
    {
        private readonly FormupContext _context;

        public GetWorkCasesHandler(FormupContext context) => _context = context;

        public async Task<PagedResult<WorkCaseList>> Handle(GetWorkCasesQuery request, CancellationToken ct)
        {
            var query = _context.WorkCases.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Relation))
                query = query.Where(x => x.Relation == request.Relation);

            if (request.ForwarderId.HasValue)
                query = query.Where(x => x.Forwarder.Id.Value == request.ForwarderId.Value);

            if (request.ClientId.HasValue)
                query = query.Where(x => x.Client.Id.Value == request.ClientId.Value);

            if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
                query = query.Where(x => x.Name.Contains(request.SearchPhrase));

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new WorkCaseList
                {
                    Id = x.Id.Value,
                    Name = x.Name,
                    Amount = x.Amount,
                    Relation = x.Relation,
                    ForwarderName = x.Forwarder.Surname,
                    ClientName = x.Client.Name
                })
                .ToListAsync(ct);

            return new PagedResult<WorkCaseList>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}