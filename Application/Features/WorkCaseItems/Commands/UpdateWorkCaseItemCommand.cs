using Application.Common.Results;
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
    ) : IRequest<AppResult<Unit>>;

    public class UpdateWorkCaseItemHandler(FormupContext context)
        : IRequestHandler<UpdateWorkCaseItemCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<Unit>> Handle(UpdateWorkCaseItemCommand request, CancellationToken ct)
        {
            var workCaseItem = await _context.WorkCaseItems
                .Include(x => x.WorkCase)
                .Include(x => x.Invoice)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.WorkCaseItemId), ct);

            if (workCaseItem == null) return AppResult<Unit>.Failure("WORK_CASE_ITEM.NOT_FOUND");
            if (workCaseItem.IsInvoiced) return AppResult<Unit>.Failure("WORK_CASE_ITEM.ALREADY_INVOICED");

            var workCase = workCaseItem.WorkCase;

            if (workCase == null) return AppResult<Unit>.Failure("WORK_CASE.NOT_FOUND");

            var otherItemsTotalUsage = await _context.WorkCaseItems
                .Where(x => x.WorkCase.Id.Equals(workCase.Id) && !x.Id.Equals(request.WorkCaseItemId))
                .SumAsync(x => x.Amount, ct);

            var availableBudget = workCase.Amount - otherItemsTotalUsage;

            if (request.Amount > availableBudget)
            {
                return AppResult<Unit>.Failure(
                    "WORK_CASE.VALIDATION.BUDGET_EXCEEDED",
                    new { ExceededBy = request.Amount - availableBudget }
                );
            }

            workCaseItem.Name = request.Name;
            workCaseItem.Amount = request.Amount;
            workCaseItem.Currency = request.Currency;
            workCaseItem.Tax = request.Tax;

            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
