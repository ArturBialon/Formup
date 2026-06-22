using Application.Features.WorkCaseItems.Commands;
using FluentValidation;

namespace Application.Features.WorkCaseItems.Validators
{
    public class DeleteWorkCaseItemCommandValidator : AbstractValidator<DeleteWorkCaseItemCommand>
    {
        public DeleteWorkCaseItemCommandValidator()
        {
            RuleFor(x => x.WorkCaseItemId)
                .NotEmpty().WithErrorCode("WORK_CASE_ITEM.VALIDATION.ID.REQUIRED");
        }
    }
}
