using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCaseItems.Queries
{
    public record GetWorkCaseItemsQuery(Guid WorkCaseId) : IRequest<AppResult<IReadOnlyCollection<WorkCaseItemResponse>>>;

    public class GetWorkCaseItemsQueryHandler(FormupContext context)
        : IRequestHandler<GetWorkCaseItemsQuery, AppResult<IReadOnlyCollection<WorkCaseItemResponse>>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<IReadOnlyCollection<WorkCaseItemResponse>>> Handle(GetWorkCaseItemsQuery request, CancellationToken ct)
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
                    IsInvoiced = x.Invoice != null ? true : false,
                    Cost = x.Cost != null ? new CostResponse
                    {
                        Id = x.Cost.Id.Value,
                        Name = x.Cost.Name,
                        Amount = x.Cost.Amount,
                        Currency = x.Cost.CurrencyCode,
                        Tax = x.Cost.Tax,
                        IssueDate = x.Cost.IssueDate,
                        ServiceDate = x.Cost.ServiceDate,
                        ServiceContractorName = x.Cost.ServiceContractor.Name + " " + x.Cost.ServiceContractor.Tax
                    } : null
                })
                .ToListAsync(ct);

            return AppResult<IReadOnlyCollection<WorkCaseItemResponse>>.Success(items);
        }
    }
}
