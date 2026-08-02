using Application.Common.CurrencyServices;
using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Invoices.Commands
{
    public record UpdateInvoiceCommand(
        Guid InvoiceId,
        DateTime IssueDate,
        DateTime ServiceDate,
        decimal TaxRate,
        string TargetCurrency,
        decimal? ManualExchangeRate,
        List<Guid> WorkCaseItemIds,
        List<Guid> WorkCaseItemsToDetachIds
    ) : IRequest<AppResult<Unit>>;

    public class UpdateInvoiceHandler(FormupContext context, ICurrencyConverterService currencyConverter)
        : IRequestHandler<UpdateInvoiceCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrencyConverterService _currencyConverter = currencyConverter;

        public async Task<AppResult<Unit>> Handle(UpdateInvoiceCommand request, CancellationToken ct)
        {
            var invoice = await _context.Invoices
                .Include(x => x.WorkCase)
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.InvoiceId), ct);

            if (invoice == null) return AppResult<Unit>.Failure("INVOICE.NOT_FOUND");
            if (invoice.IsPaid) return AppResult<Unit>.Failure("INVOICE.CANNOT_DELETE_PAID");

            var requestedItems = await _context.WorkCaseItems
                .Where(x => x.WorkCase.Id.Equals(invoice.WorkCase.Id) && request.WorkCaseItemIds.Contains(x.Id))
                .ToListAsync(ct);

            if (requestedItems.Count != request.WorkCaseItemIds.Count)
                return AppResult<Unit>.Failure("INVOICE.SOME_ITEMS_NOT_FOUND");

            if (requestedItems.Any(x => x.Invoice != null && x.Invoice.Id.Value != invoice.Id.Value))
                return AppResult<Unit>.Failure("INVOICE.SOME_ITEMS_ALREADY_INVOICED");


            var itemsToDetach = await _context.WorkCaseItems
                .Include(x => x.Invoice)
                .Where(x => x.WorkCase.Id.Equals(invoice.WorkCase.Id) && request.WorkCaseItemsToDetachIds.Contains(x.Id))
                .ToListAsync(ct);

            foreach (var item in itemsToDetach)
            {
                item.Invoice = null;
            }

            foreach (var item in requestedItems)
            {
                item.Invoice = invoice;
            }

            var conversionInputs = requestedItems
                .Select(x => new CurrencyConversionInput(x.Id.Value, x.AmountToInvoice, x.CurrencyCodeInvoice))
                .ToList();

            var conversionResult = await _currencyConverter.ConvertCurrenciesAsync(
                conversionInputs, request.TargetCurrency, request.ManualExchangeRate, request.ServiceDate, ct);

            if (conversionResult.IsFailure)
            {
                return AppResult<Unit>.Failure(conversionResult.ErrorCode, conversionResult.ErrorData);
            }

            var conversionData = conversionResult.Value!;

            invoice.Amount = conversionData.TotalTargetAmount;
            invoice.CurrencyCode = conversionData.TargetCurrency;
            invoice.IssueDateUtc = request.IssueDate;
            invoice.ServiceDateUtc = request.ServiceDate;
            invoice.Tax = request.TaxRate;

            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
