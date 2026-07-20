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
                .AnyAsync(x => x.Id.Equals(request.WorkCaseId), ct);

            if (!workCaseExists) return AppResult<IReadOnlyCollection<WorkCaseItemResponse>>.Failure("WORK_CASE.NOT_FOUND");

            var items = await _context.WorkCaseItems
                .AsNoTracking()
                .Where(x => x.WorkCase.Id.Equals(request.WorkCaseId))
                .Select(x => new WorkCaseItemResponse
                {
                    Id = x.Id.Value,
                    Name = x.Name,
                    AmountToInvoice = x.AmountToInvoice,
                    InvoiceCurrencyCode = x.CurrencyCodeInvoice,
                    CostAmount = x.CostAmountNet,
                    CostCurrencyCode = x.CurrencyCodeCost,
                    Tax = x.TaxInvoice,
                    CreatedAtUtc = x.CreatedAtUtc,
                    InvoiceId = x.Invoice != null ? x.Invoice.Id : null,
                    Cost = x.Cost != null ? new CostResponse
                    {
                        Id = x.Cost.Id.Value,
                        Name = x.Cost.Name,
                        Amount = x.Cost.Amount,
                        Currency = x.Cost.CurrencyCode,
                        Tax = x.Cost.Tax,
                        IssueDate = x.Cost.IssueDate,
                        ServiceDate = x.Cost.ServiceDate
                    } : null
                })
                .ToListAsync(ct);

            return AppResult<IReadOnlyCollection<WorkCaseItemResponse>>.Success(items);
        }
    }
}
