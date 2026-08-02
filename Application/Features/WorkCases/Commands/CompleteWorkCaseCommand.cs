using Application.Common.CurrencyServices;
using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Commands
{
    public record CompleteWorkCaseCommand(Guid WorkCaseId) : IRequest<AppResult<Unit>>;

    public class CompleteWorkCaseHandler(FormupContext context, ICurrencyConverterService currencyConverterService) : IRequestHandler<CompleteWorkCaseCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrencyConverterService _currencyConverterService = currencyConverterService;

        public async Task<AppResult<Unit>> Handle(CompleteWorkCaseCommand request, CancellationToken ct)
        {
            var workCase = await _context.WorkCases
                .Include(x => x.WorkCaseItems)
                .FirstOrDefaultAsync(wc => wc.Id.Equals(request.WorkCaseId), cancellationToken: ct);
            if (workCase == null) return AppResult<Unit>.Failure("WORK_CASE.NOT_FOUND");

            var conversionInputs = workCase.WorkCaseItems
                .Select(x => new CurrencyConversionInput(x.Id.Value, x.AmountToInvoice, x.CurrencyCodeInvoice))
                .ToList();

            var conversionResult = await _currencyConverterService.ConvertCurrenciesAsync(conversionInputs, workCase.CurrencyCode, null, DateTime.UtcNow, ct);
            if (conversionResult.IsFailure) return AppResult<Unit>.Failure(conversionResult.ErrorCode, conversionResult.ErrorData);

            workCase.Amount = conversionResult.Value!.TotalTargetAmount;
            workCase.AmountInPln = conversionResult.Value!.TotalAmountInPln;
            workCase.IsCompleted = true;

            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
