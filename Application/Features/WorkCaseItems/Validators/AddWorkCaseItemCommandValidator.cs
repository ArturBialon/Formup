using Application.Features.WorkCaseItems.Commands;
using FluentValidation;

namespace Application.Features.WorkCaseItems.Validators
{
    public class AddWorkCaseItemCommandValidator : AbstractValidator<AddWorkCaseItemCommand>
    {
        public AddWorkCaseItemCommandValidator()
        {
            RuleFor(x => x.WorkCaseId)
                .NotEmpty().WithErrorCode("WORK_CASE_ITEM.VALIDATION.WORK_CASE_ID.REQUIRED");

            RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode("WORK_CASE_ITEM.VALIDATION.NAME.REQUIRED")
                .MaximumLength(150).WithErrorCode("WORK_CASE_ITEM.VALIDATION.NAME.TOO_LONG");

            RuleFor(x => x.Amount)
                .NotEmpty().WithErrorCode("WORK_CASE_ITEM.VALIDATION.AMOUNT.REQUIRED")
                .GreaterThan(0).WithErrorCode("WORK_CASE_ITEM.VALIDATION.AMOUNT.MUST_BE_POSITIVE");

            RuleFor(x => x.Currency)
                .NotEmpty().WithErrorCode("WORK_CASE_ITEM.VALIDATION.CURRENCY.REQUIRED")
                .MaximumLength(3).WithErrorCode("WORK_CASE_ITEM.VALIDATION.CURRENCY.TOO_LONG");

            RuleFor(x => x.Tax)
                .NotNull().WithErrorCode("WORK_CASE_ITEM.VALIDATION.TAX.REQUIRED")
                .GreaterThanOrEqualTo(0).WithErrorCode("WORK_CASE_ITEM.VALIDATION.TAX.INVALID");
        }
    }
}
