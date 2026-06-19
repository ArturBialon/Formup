using Application.Features.WorkCases.Commands;
using FluentValidation;

namespace Application.Features.WorkCases.Validators
{
    public class AbandonWorkCaseCommandValidator : AbstractValidator<AbandonWorkCaseCommand>
    {
        public AbandonWorkCaseCommandValidator()
        {
            RuleFor(x => x.WorkCaseId)
                .NotEmpty().WithErrorCode("WORK_CASE.VALIDATION.ID.REQUIRED");
        }
    }
}