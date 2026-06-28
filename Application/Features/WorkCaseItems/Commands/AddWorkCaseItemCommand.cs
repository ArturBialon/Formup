using Application.Common.Results;
using Application.DTOs.Response;
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
    ) : IRequest<AppResult<WorkCaseItemResponse>>;

    public class AddWorkCaseItemHandler(FormupContext context)
        : IRequestHandler<AddWorkCaseItemCommand, AppResult<WorkCaseItemResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<WorkCaseItemResponse>> Handle(AddWorkCaseItemCommand request, CancellationToken ct)
        {
            var workCase = await _context.WorkCases
                .FirstOrDefaultAsync(x => x.Id.Value == request.WorkCaseId, ct);

            if (workCase == null) return AppResult<WorkCaseItemResponse>.Failure("WORK_CASE.NOT_FOUND");

            var currentWorkCaseUsage = await _context.WorkCaseItems
                .Where(x => x.WorkCase.Id.Value == request.WorkCaseId)
                .SumAsync(x => x.Amount, ct);

            var availableBudget = workCase.Amount - currentWorkCaseUsage;

            if (request.Amount > availableBudget)
            {
                return AppResult<WorkCaseItemResponse>.Failure(
                    "WORK_CASE.VALIDATION.BUDGET_EXCEEDED",
                    new { ExceededBy = request.Amount - availableBudget }
                );
            }

            var newItem = new WorkCaseItem
            {
                Name = request.Name,
                Amount = request.Amount,
                Currency = request.Currency,
                Tax = request.Tax,
                WorkCase = workCase
            };

            _context.WorkCaseItems.Add(newItem);
            await _context.SaveChangesAsync(ct);

            var response = new WorkCaseItemResponse
            {
                Id = newItem.Id.Value,
                Name = newItem.Name,
                Amount = newItem.Amount,
                Currency = newItem.Currency,
                Tax = newItem.Tax,
                CreatedAt = newItem.CreatedAt,
                InvoiceId = null
            };

            return AppResult<WorkCaseItemResponse>.Success(response);
        }
    }
}
