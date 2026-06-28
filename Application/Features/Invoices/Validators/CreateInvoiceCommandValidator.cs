using Application.Features.Invoices.Commands;
using FluentValidation;

namespace Application.Features.Invoices.Validators
{
    public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
    {
        public CreateInvoiceCommandValidator()
        {
            RuleFor(x => x.WorkCaseId)
                .NotEmpty().WithErrorCode("INVOICE.VALIDATION.WORK_CASE_ID_REQUIRED");

            RuleFor(x => x.ServiceDate)
                .NotEmpty().WithErrorCode("INVOICE.VALIDATION.SERVICE_DATE_REQUIRED")
                .LessThanOrEqualTo(_ => DateTime.Now.Date).WithErrorCode("INVOICE.VALIDATION.SERVICE_DATE_FUTURE");

            RuleFor(x => x.TaxRate)
                .GreaterThanOrEqualTo(0).WithErrorCode("INVOICE.VALIDATION.TAX_RATE_MIN_VALUE")
                .LessThan(1000).WithErrorCode("INVOICE.VALIDATION.TAX_RATE_TOO_LARGE");

            RuleFor(x => x.TargetCurrency)
                .NotEmpty().WithErrorCode("INVOICE.VALIDATION.CURRENCY_REQUIRED")
                .MaximumLength(3).WithErrorCode("INVOICE.VALIDATION.CURRENCY_TOO_LONG");

            RuleFor(x => x.ManualExchangeRate)
                .GreaterThan(0).When(x => x.ManualExchangeRate.HasValue)
                .WithErrorCode("INVOICE.VALIDATION.MANUAL_EXCHANGE_RATE_MUST_BE_POSITIVE");

            RuleFor(x => x.WorkCaseItemIds)
                .NotEmpty().WithErrorCode("INVOICE.VALIDATION.ITEMS_REQUIRED")
                .Must(x => x != null && x.Count > 0).WithErrorCode("INVOICE.VALIDATION.ITEMS_EMPTY");

            RuleForEach(x => x.WorkCaseItemIds)
                .NotEmpty().WithErrorCode("INVOICE.VALIDATION.INVALID_ITEM_ID");
        }
    }
}
