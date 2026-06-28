using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCaseItems.Commands
{
    public record UpdateWorkCaseItemCommand(
        Guid WorkCaseItemId,
        string Name,
        decimal Amount,
        string Currency,
        decimal Tax
    ) : IRequest<AppResult<WorkCaseItemResponse>>;

    public class UpdateWorkCaseItemHandler(FormupContext context)
        : IRequestHandler<UpdateWorkCaseItemCommand, AppResult<WorkCaseItemResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<WorkCaseItemResponse>> Handle(UpdateWorkCaseItemCommand request, CancellationToken ct)
        {
            var workCaseItem = await _context.WorkCaseItems
                .Include(x => x.WorkCase)
                .Include(x => x.Invoice)
                .FirstOrDefaultAsync(x => x.Id.Value == request.WorkCaseItemId, ct);

            if (workCaseItem == null) return AppResult<WorkCaseItemResponse>.Failure("WORK_CASE_ITEM.NOT_FOUND");
            if (workCaseItem.IsInvoiced) return AppResult<WorkCaseItemResponse>.Failure("WORK_CASE_ITEM.VALIDATION.ALREADY_INVOICED");

            var workCase = workCaseItem.WorkCase;

            if (workCase == null) return AppResult<WorkCaseItemResponse>.Failure("WORK_CASE.NOT_FOUND");

            var otherItemsTotalUsage = await _context.WorkCaseItems
                .Where(x => x.WorkCase.Id.Value == workCase.Id.Value && x.Id.Value != request.WorkCaseItemId)
                .SumAsync(x => x.Amount, ct);

            var availableBudget = workCase.Amount - otherItemsTotalUsage;

            if (request.Amount > availableBudget)
            {
                return AppResult<WorkCaseItemResponse>.Failure(
                    "WORK_CASE.VALIDATION.BUDGET_EXCEEDED",
                    new { ExceededBy = request.Amount - availableBudget }
                );
            }

            workCaseItem.Name = request.Name;
            workCaseItem.Amount = request.Amount;
            workCaseItem.Currency = request.Currency;
            workCaseItem.Tax = request.Tax;

            await _context.SaveChangesAsync(ct);

            var response = new WorkCaseItemResponse
            {
                Id = workCaseItem.Id.Value,
                Name = workCaseItem.Name,
                Amount = workCaseItem.Amount,
                Currency = workCaseItem.Currency,
                Tax = workCaseItem.Tax,
                CreatedAt = workCaseItem.CreatedAt,
                InvoiceId = null
            };

            return AppResult<WorkCaseItemResponse>.Success(response);
        }
    }
}
