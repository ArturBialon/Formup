using Application.Features.Invoices.Commands;
using FluentValidation;

namespace Application.Features.Invoices.Validators
{
    public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
    {
        public UpdateInvoiceCommandValidator()
        {
            RuleFor(x => x.InvoiceId).NotEmpty().WithErrorCode("INVOICE.VALIDATION.ID.REQUIRED");
            RuleFor(x => x.IssueDate).NotEmpty().WithErrorCode("INVOICE.VALIDATION.ISSUE_DATE.REQUIRED");
            RuleFor(x => x.ServiceDate).NotEmpty().WithErrorCode("INVOICE.VALIDATION.SERVICE_DATE.REQUIRED");
            RuleFor(x => x.TaxRate).GreaterThanOrEqualTo(0).WithErrorCode("INVOICE.VALIDATION.TAX_RATE.INVALID");
            RuleFor(x => x.TargetCurrency).NotEmpty().MaximumLength(3).WithErrorCode("INVOICE.VALIDATION.CURRENCY.INVALID");
            RuleFor(x => x.WorkCaseItemIds).NotEmpty().WithErrorCode("INVOICE.VALIDATION.ITEMS.REQUIRED");
        }
    }
}
