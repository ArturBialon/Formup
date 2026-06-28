using Application.Common.CurrencyServices;
using Application.Common.Results;
using Application.DTOs.Response;
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
    ) : IRequest<AppResult<InvoiceResponse>>;

    public class UpdateInvoiceHandler(FormupContext context, ICurrencyConverterService currencyConverter)
        : IRequestHandler<UpdateInvoiceCommand, AppResult<InvoiceResponse>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrencyConverterService _currencyConverter = currencyConverter;

        public async Task<AppResult<InvoiceResponse>> Handle(UpdateInvoiceCommand request, CancellationToken ct)
        {
            var invoice = await _context.Invoices
                .Include(x => x.WorkCase)
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id.Value == request.InvoiceId, ct);

            if (invoice == null) return AppResult<InvoiceResponse>.Failure("INVOICE.NOT_FOUND");

            var requestedItems = await _context.WorkCaseItems
                .Include(x => x.Invoice)
                .Where(x => x.WorkCase.Id == invoice.WorkCase.Id && request.WorkCaseItemIds.Contains(x.Id.Value))
                .ToListAsync(ct);

            if (requestedItems.Count != request.WorkCaseItemIds.Count)
                return AppResult<InvoiceResponse>.Failure("INVOICE.VALIDATION.SOME_ITEMS_NOT_FOUND_IN_WORK_CASE");

            if (requestedItems.Any(x => x.Invoice != null && x.Invoice.Id.Value != invoice.Id.Value))
                return AppResult<InvoiceResponse>.Failure("INVOICE.VALIDATION.SOME_ITEMS_ALREADY_INVOICED_ELSEWHERE");


            var itemsToDetach = await _context.WorkCaseItems
                .Include(x => x.Invoice)
                .Where(x => x.WorkCase.Id == invoice.WorkCase.Id && request.WorkCaseItemsToDetachIds.Contains(x.Id.Value))
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
                .Select(x => new CurrencyConversionInput(x.Id.Value, x.Amount, x.Currency))
                .ToList();

            var conversionResult = await _currencyConverter.ConvertCurrenciesAsync(
                conversionInputs, request.TargetCurrency, request.ManualExchangeRate, request.ServiceDate, ct);

            if (conversionResult.IsFailure)
            {
                return AppResult<InvoiceResponse>.Failure(conversionResult.ErrorCode, conversionResult.ErrorData);
            }

            var conversionData = conversionResult.Value!;

            invoice.Amount = conversionData.TotalTargetAmount;
            invoice.Currency = conversionData.TargetCurrency;
            invoice.IssueDate = request.IssueDate;
            invoice.ServiceDate = request.ServiceDate;
            invoice.Tax = request.TaxRate;

            await _context.SaveChangesAsync(ct);

            return AppResult<InvoiceResponse>.Success(new InvoiceResponse
            {
                Id = invoice.Id.Value,
                InvoiceNumber = invoice.InvoiceNumber,
                Amount = invoice.Amount,
                Currency = invoice.Currency,
                IssueDate = invoice.IssueDate,
                ServiceDate = invoice.ServiceDate,
                Tax = invoice.Tax,
                WorkCaseId = invoice.WorkCase.Id.Value,
                ClientId = invoice.Client.Id.Value,
                InvoicedItemIds = [.. requestedItems.Select(x => x.Id.Value)]
            });
        }
    }
}
