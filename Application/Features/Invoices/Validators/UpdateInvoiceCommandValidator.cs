using Application.Features.Invoices.Commands;
using FluentValidation;

namespace Application.Features.Invoices.Validators
{
    public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
    {
        public UpdateInvoiceCommandValidator()
        {
            RuleFor(x => x.InvoiceId)
                .NotEmpty().WithErrorCode("INVOICE.VALIDATION.ID_REQUIRED");

            RuleFor(x => x.IssueDate)
                .NotEmpty().WithErrorCode("INVOICE.VALIDATION.ISSUE_DATE_REQUIRED")
                .LessThanOrEqualTo(_ => DateTime.Now.Date).WithErrorCode("INVOICE.VALIDATION.ISSUE_DATE_FUTURE");

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
                .NotEmpty().WithErrorCode("INVOICE.VALIDATION.ITEMS_REQUIRED");
        }
    }
}
