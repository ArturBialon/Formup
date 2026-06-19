using Application.Features.ServiceContractors.Commands;
using FluentValidation;

namespace Application.Features.ServiceContractors.Validators
{
    public class DeleteServiceContractorCommandValidator : AbstractValidator<DeleteServiceContractorCommand>
    {
        public DeleteServiceContractorCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithErrorCode("CONTRACTOR.VALIDATION.ID.REQUIRED");
        }
    }
}
