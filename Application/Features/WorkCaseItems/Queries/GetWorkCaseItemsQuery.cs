using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCaseItems.Queries
{
    public record GetWorkCaseItemsQuery(Guid WorkCaseId) : IRequest<IAppResult<IReadOnlyCollection<WorkCaseItemResponse>>>;

    public class GetWorkCaseItemsQueryHandler(FormupContext context)
        : IRequestHandler<GetWorkCaseItemsQuery, IAppResult<IReadOnlyCollection<WorkCaseItemResponse>>>
    {
        private readonly FormupContext _context = context;

        public async Task<IAppResult<IReadOnlyCollection<WorkCaseItemResponse>>> Handle(GetWorkCaseItemsQuery request, CancellationToken ct)
        {
            var workCaseExists = await _context.WorkCases
                .AnyAsync(x => x.Id.Value == request.WorkCaseId, ct);

            if (!workCaseExists)
            {
                return AppResult<IReadOnlyCollection<WorkCaseItemResponse>>.Failure("WORK_CASE.NOT_FOUND");
            }

            var items = await _context.WorkCaseItems
                .AsNoTracking()
                .Where(x => x.WorkCase.Id.Value == request.WorkCaseId)
                .Select(x => new WorkCaseItemResponse
                {
                    Id = x.Id.Value,
                    Name = x.Name,
                    Amount = x.Amount,
                    Currency = x.Currency,
                    Tax = x.Tax,
                    CreatedAt = x.CreatedAt,
                    InvoiceId = x.Invoice != null ? x.Invoice.Id : null,
                    Costs = x.Costs.Select(c => new CostResponse
                    {
                        Id = c.Id.Value,
                        Name = c.Name,
                        Amount = c.Amount,
                        Currency = c.Currency,
                        Tax = c.Tax,
                        IssueDate = c.IssueDate,
                        ServiceDate = c.ServiceDate
                    }).ToList()
                })
                .ToListAsync(ct);

            return AppResult<IReadOnlyCollection<WorkCaseItemResponse>>.Success(items);
        }
    }
}
