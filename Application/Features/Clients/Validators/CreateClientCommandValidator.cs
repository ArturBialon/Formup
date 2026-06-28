using Application.Features.Clients.Commands;
using FluentValidation;

namespace Application.Features.Clients.Validators
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(x => x.Tax)
                .NotEmpty().WithErrorCode("CLIENT.VALIDATION.TAX_REQUIRED")
                .MaximumLength(20).WithErrorCode("CLIENT.VALIDATION.TAX_TOO_LONG");

            RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode("CLIENT.VALIDATION.NAME_REQUIRED")
                .MaximumLength(50).WithErrorCode("CLIENT.VALIDATION.NAME_TOO_LONG");

            RuleFor(x => x.Street)
                .NotEmpty().WithErrorCode("CLIENT.VALIDATION.STREET_REQUIRED")
                .MaximumLength(80).WithErrorCode("CLIENT.VALIDATION.STREET_TOO_LONG");

            RuleFor(x => x.Zip)
                .NotEmpty().WithErrorCode("CLIENT.VALIDATION.ZIP_REQUIRED")
                .MaximumLength(10).WithErrorCode("CLIENT.VALIDATION.ZIP_TOO_LONG");

            RuleFor(x => x.Coutry)
                .NotEmpty().WithErrorCode("CLIENT.VALIDATION.COUNTRY_REQUIRED")
                .MaximumLength(54).WithErrorCode("CLIENT.VALIDATION.COUNTRY_TOO_LONG");

            RuleFor(x => x.Credit)
                .GreaterThanOrEqualTo(0).WithErrorCode("CLIENT.VALIDATION.CREDIT_MIN_VALUE");

            RuleFor(x => x.Currency)
                .NotEmpty().WithErrorCode("CLIENT.VALIDATION.CURRENCY_REQUIRED")
                .MaximumLength(3).WithErrorCode("CLIENT.VALIDATION.CURRENCY_TOO_LONG");
        }
    }
}
