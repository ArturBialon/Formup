using FluentValidation;

namespace Application.Features.WorkCases.Commands
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