using Application.Features.Costs.Commands;
using FluentValidation;

namespace Application.Features.Costs.Validators
{
    public class UpdateCostCommandValidator : AbstractValidator<UpdateCostCommand>
    {
        public UpdateCostCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithErrorCode("COST.VALIDATION.ID_REQUIRED");

            RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode("COST.VALIDATION.NAME_REQUIRED")
                .MaximumLength(50).WithErrorCode("COST.VALIDATION.NAME_TOO_LONG");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithErrorCode("COST.VALIDATION.AMOUNT_MUST_BE_POSITIVE")
                .LessThan(1000000000).WithErrorCode("COST.VALIDATION.AMOUNT_TOO_LARGE");

            RuleFor(x => x.Currency)
                .NotEmpty().WithErrorCode("COST.VALIDATION.CURRENCY_REQUIRED")
                .MaximumLength(3).WithErrorCode("COST.VALIDATION.CURRENCY_TOO_LONG");

            RuleFor(x => x.Tax)
                .GreaterThanOrEqualTo(0).WithErrorCode("COST.VALIDATION.TAX_MIN_VALUE")
                .LessThan(1000).WithErrorCode("COST.VALIDATION.TAX_TOO_LARGE");

            RuleFor(x => x.IssueDate)
                .NotEmpty().WithErrorCode("COST.VALIDATION.ISSUE_DATE_REQUIRED")
                .LessThanOrEqualTo(_ => DateTime.Now.Date).WithErrorCode("COST.VALIDATION.ISSUE_DATE_FUTURE");

            RuleFor(x => x.ServiceDate)
                .NotEmpty().WithErrorCode("COST.VALIDATION.SERVICE_DATE_REQUIRED")
                .LessThanOrEqualTo(_ => DateTime.Now.Date).WithErrorCode("COST.VALIDATION.SERVICE_DATE_FUTURE");

            RuleFor(x => x.WorkCaseItemId)
                .NotEmpty().WithErrorCode("COST.VALIDATION.WORK_CASE_ITEM_ID_REQUIRED");

            RuleFor(x => x.ServiceContractorId)
                .NotEmpty().WithErrorCode("COST.VALIDATION.CONTRACTOR_ID_REQUIRED");
        }
    }
}
