using Application.Common.CurrencyServices;
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
        string InvoiceCurrencyCode,
        decimal CostAmountNet,
        string CostCurrencyCode,
        decimal Tax
    ) : IRequest<AppResult<Unit>>;

    public class UpdateWorkCaseItemHandler(FormupContext context, ICurrencyConverterService currencyConverterService)
        : IRequestHandler<UpdateWorkCaseItemCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrencyConverterService _currencyConverterService = currencyConverterService;

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

            var otherWorkCaseItems = await _context.WorkCaseItems
                .Where(x => x.WorkCase.Id.Equals(workCase.Id) && !x.Id.Equals(request.WorkCaseItemId))
                .ToListAsync(ct);

            var currencyConversionInputs = otherWorkCaseItems
                .Select(x => new CurrencyConversionInput(x.Id, x.AmountToInvoice, x.CurrencyCodeInvoice))
                .ToList();

            var otherItemsUsageResult = await _currencyConverterService.ConvertCurrenciesAsync(currencyConversionInputs, "PLN", null, DateTime.UtcNow, ct);
            if (otherItemsUsageResult.IsFailure)
                return AppResult<Unit>.Failure(otherItemsUsageResult.ErrorCode, otherItemsUsageResult.ErrorData);

            var requestedAmountInPln = await _currencyConverterService.ConvertToTargetCurrency(request.Amount, request.InvoiceCurrencyCode, "PLN", DateTime.UtcNow, ct);
            if (requestedAmountInPln.IsFailure)
                return AppResult<Unit>.Failure(requestedAmountInPln.ErrorCode, requestedAmountInPln.ErrorData);

            var availableBudgetInPln = workCase.AmountInPln - otherItemsUsageResult.Value!.TotalTargetAmount;

            if (requestedAmountInPln.Value > availableBudgetInPln)
            {
                var exceededByPln = requestedAmountInPln.Value - availableBudgetInPln;
                var exceededByTargetCurrency = await _currencyConverterService.ConvertToTargetCurrency(exceededByPln, "PLN", request.InvoiceCurrencyCode, DateTime.UtcNow, ct);

                return AppResult<Unit>.Failure(
                    "WORK_CASE.VALIDATION.BUDGET_EXCEEDED",
                    new { ExceededBy = exceededByTargetCurrency.Value, Currency = request.InvoiceCurrencyCode }
                );
            }

            workCaseItem.Name = request.Name;
            workCaseItem.AmountToInvoice = request.Amount;
            workCaseItem.CurrencyCodeInvoice = request.InvoiceCurrencyCode;
            workCaseItem.CostAmountNet = request.CostAmountNet;
            workCaseItem.CurrencyCodeCost = request.CostCurrencyCode;
            workCaseItem.TaxInvoice = request.Tax;

            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}