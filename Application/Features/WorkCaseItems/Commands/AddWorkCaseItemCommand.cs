using Application.Common.Results;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCaseItems.Commands
{
    public record AddWorkCaseItemCommand(
        Guid WorkCaseId,
        string Name,
        decimal Amount,
        string Currency,
        decimal Tax
    ) : IRequest<AppResult<Unit>>;

    public class AddWorkCaseItemHandler(FormupContext context)
        : IRequestHandler<AddWorkCaseItemCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<Unit>> Handle(AddWorkCaseItemCommand request, CancellationToken ct)
        {
            var workCase = await _context.WorkCases
                .FirstOrDefaultAsync(x => x.Id.Equals(request.WorkCaseId), ct);

            if (workCase == null) return AppResult<Unit>.Failure("WORK_CASE.NOT_FOUND");

            var currentWorkCaseUsage = await _context.WorkCaseItems
                .Where(x => x.WorkCase.Id.Equals(request.WorkCaseId))
                .SumAsync(x => x.Amount, ct);

            var availableBudget = workCase.Amount - currentWorkCaseUsage;

            if (request.Amount > availableBudget)
            {
                return AppResult<Unit>.Failure(
                    "WORK_CASE.VALIDATION.BUDGET_EXCEEDED",
                    new { ExceededBy = request.Amount - availableBudget }
                );
            }

            var newItem = new WorkCaseItem
            {
                Name = request.Name,
                Amount = request.Amount,
                CurrencyCode = request.Currency,
                Tax = request.Tax,
                WorkCase = workCase
            };

            _context.WorkCaseItems.Add(newItem);
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
