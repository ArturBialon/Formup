using Application.Features.Users.Commands;
using FluentValidation;

namespace Application.Features.Forwarders.Validators
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithErrorCode("REGISTRATION.VALIDATION.EMAIL_REQUIRED")
                .EmailAddress().WithErrorCode("REGISTRATION.VALIDATION.EMAIL_INVALID");

            RuleFor(x => x.Name).NotEmpty().WithErrorCode("REGISTRATION.VALIDATION.NAME_REQUIRED");
            RuleFor(x => x.Surname).NotEmpty().WithErrorCode("REGISTRATION.VALIDATION.SURNAME_REQUIRED");

            RuleFor(x => x.Prefix)
                .NotEmpty().WithErrorCode("REGISTRATION.VALIDATION.INITIALS_REQUIRED")
                .Length(2, 5).WithErrorCode("REGISTRATION.VALIDATION.INITIALS_LENGTH_INVALID");

            RuleFor(x => x.Password)
                .NotEmpty().WithErrorCode("REGISTRATION.VALIDATION.PASSWORD_REQUIRED")
                .MinimumLength(6).WithErrorCode("REGISTRATION.VALIDATION.PASSWORD_TOO_SHORT")
                .MaximumLength(30).WithErrorCode("REGISTRATION.VALIDATION.PASSWORD_TOO_LONG");

            RuleFor(x => x.Role).IsInEnum().WithErrorCode("REGISTRATION.VALIDATION.ROLE_INVALID");
        }
    }
}
