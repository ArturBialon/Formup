using Application.Common.CurrencyServices;
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
        string InvoiceCurrencyCode,
        decimal CostAmountNet,
        string CostCurrencyCode,
        decimal Tax
    ) : IRequest<AppResult<Unit>>;

    public class AddWorkCaseItemHandler(FormupContext context, ICurrencyConverterService currencyConverterService)
        : IRequestHandler<AddWorkCaseItemCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrencyConverterService _currencyConverterService = currencyConverterService;

        public async Task<AppResult<Unit>> Handle(AddWorkCaseItemCommand request, CancellationToken ct)
        {
            var workCase = await _context.WorkCases
                .FirstOrDefaultAsync(x => x.Id.Equals(request.WorkCaseId), ct);

            if (workCase == null) return AppResult<Unit>.Failure("WORK_CASE.NOT_FOUND");

            var workCaseItems = await _context.WorkCaseItems.Where(x => x.WorkCase.Id.Equals(request.WorkCaseId)).ToListAsync(ct);
            var currencyConversionInputs = workCaseItems.Select(x => new CurrencyConversionInput(x.Id, x.AmountToInvoice, x.CurrencyCodeInvoice)).ToList();

            var currentWorkCaseUsageResult = await _currencyConverterService.ConvertCurrenciesAsync(currencyConversionInputs, "PLN", null, DateTime.UtcNow, ct);
            if (currentWorkCaseUsageResult.IsFailure)
                return AppResult<Unit>.Failure(currentWorkCaseUsageResult.ErrorCode, currentWorkCaseUsageResult.ErrorData);

            var requestedAmountInPln = await _currencyConverterService.ConvertToTargetCurrency(request.Amount, request.InvoiceCurrencyCode, "PLN", DateTime.UtcNow, ct);
            if (requestedAmountInPln.IsFailure)
                return AppResult<Unit>.Failure(requestedAmountInPln.ErrorCode, requestedAmountInPln.ErrorData);

            var availableBudgetInPln = workCase.AmountInPln - currentWorkCaseUsageResult.Value!.TotalTargetAmount;

            if (requestedAmountInPln.Value > availableBudgetInPln)
            {
                var exceededByInPln = requestedAmountInPln.Value - availableBudgetInPln;
                var exceededByTargetCurrency = await _currencyConverterService.ConvertToTargetCurrency(exceededByInPln, "PLN", request.InvoiceCurrencyCode, DateTime.UtcNow, ct);
                return AppResult<Unit>.Failure(
                    "WORK_CASE.VALIDATION.BUDGET_EXCEEDED",
                    new { ExceededBy = exceededByTargetCurrency.Value, request.InvoiceCurrencyCode }
                );
            }

            var newItem = new WorkCaseItem
            {
                Name = request.Name,
                AmountToInvoice = request.Amount,
                CostAmountNet = request.CostAmountNet,
                CurrencyCodeInvoice = request.InvoiceCurrencyCode,
                CurrencyCodeCost = request.CostCurrencyCode,
                TaxInvoice = request.Tax,
                WorkCase = workCase
            };

            _context.WorkCaseItems.Add(newItem);
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
