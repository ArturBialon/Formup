using Application.Common.CurrencyServices;
using Application.Common.Results;
using Application.DTOs.Response;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Invoices.Commands
{
    public record CreateInvoiceCommand(
        Guid WorkCaseId,
        DateTime ServiceDate,
        decimal TaxRate,
        string TargetCurrency,
        decimal? ManualExchangeRate,
        List<Guid> WorkCaseItemIds
    ) : IRequest<AppResult<InvoiceResponse>>;

    public class CreateInvoiceHandler(FormupContext context, ICurrencyConverterService currencyConverter)
        : IRequestHandler<CreateInvoiceCommand, AppResult<InvoiceResponse>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrencyConverterService _currencyConverter = currencyConverter;

        public async Task<AppResult<InvoiceResponse>> Handle(CreateInvoiceCommand request, CancellationToken ct)
        {
            var workCase = await _context.WorkCases.Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id.Value == request.WorkCaseId, ct);

            if (workCase == null) return AppResult<InvoiceResponse>.Failure("WORK_CASE.NOT_FOUND");

            var itemsToInvoice = await _context.WorkCaseItems.Include(x => x.Invoice)
                .Where(x => x.WorkCase.Id.Value == request.WorkCaseId && request.WorkCaseItemIds.Contains(x.Id.Value))
                .ToListAsync(ct);

            if (itemsToInvoice.Count != request.WorkCaseItemIds.Count)
                return AppResult<InvoiceResponse>.Failure("INVOICE.VALIDATION.SOME_ITEMS_NOT_FOUND");

            if (itemsToInvoice.Any(x => x.IsInvoiced))
                return AppResult<InvoiceResponse>.Failure("INVOICE.VALIDATION.SOME_ITEMS_ALREADY_INVOICED");

            var conversionItems = itemsToInvoice
                .Select(x => new CurrencyConversionInput(x.Id.Value, x.Amount, x.Currency))
                .ToList();

            var conversionResult = await _currencyConverter.ConvertCurrenciesAsync(conversionItems, request.TargetCurrency, request.ManualExchangeRate, ct);

            if (conversionResult.IsFailure)
            {
                return AppResult<InvoiceResponse>.Failure(conversionResult.ErrorCode, conversionResult.ErrorData);
            }

            var conversionData = conversionResult.Value!;
            var invoiceNumber = await CreateInvoiceNumberAsync(request, workCase.Forwarder, ct);

            var invoice = new Invoice
            {
                InvoiceNumber = invoiceNumber,
                Amount = conversionData.TotalTargetAmount,
                Currency = conversionData.TargetCurrency,
                IssueDate = DateTime.Now,
                ServiceDate = request.ServiceDate,
                Tax = request.TaxRate,
                WorkCase = workCase,
                Client = workCase.Client
            };

            foreach (var item in itemsToInvoice)
            {
                item.Invoice = invoice;
            }

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync(ct);

            return AppResult<InvoiceResponse>.Success(new InvoiceResponse
            {
                Id = invoice.Id.Value,
                Amount = invoice.Amount,
                Currency = invoice.Currency,
                IssueDate = invoice.IssueDate,
                ServiceDate = invoice.ServiceDate,
                Tax = invoice.Tax,
                WorkCaseId = workCase.Id.Value,
                ClientId = workCase.Client.Id.Value,
                InvoicedItemIds = [.. itemsToInvoice.Select(x => x.Id.Value)]
            });
        }
        private async Task<string> CreateInvoiceNumberAsync(CreateInvoiceCommand request, Forwarder forwarder, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var monthlyInvoiceAmount = await _context.Invoices
                .CountAsync(x => x.WorkCase.Forwarder.Id == forwarder.Id && x.IssueDate.Month == now.Month, ct);

            return $"FK/{monthlyInvoiceAmount + 1}/{forwarder.Prefix}/{now.Month}/{now.Year}";
        }
    }
}
