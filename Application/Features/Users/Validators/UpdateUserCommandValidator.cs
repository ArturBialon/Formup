using Application.Features.Users.Commands;
using FluentValidation;

namespace Application.Features.Users.Validators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithErrorCode("USER.VALIDATION.ID_REQUIRED");

            RuleFor(x => x.Email)
                .NotEmpty().WithErrorCode("USER.VALIDATION.EMAIL_REQUIRED")
                .EmailAddress().WithErrorCode("USER.VALIDATION.EMAIL_INVALID");

            RuleFor(x => x.Name).NotEmpty().WithErrorCode("USER.VALIDATION.NAME_REQUIRED");
            RuleFor(x => x.Surname).NotEmpty().WithErrorCode("USER.VALIDATION.SURNAME_REQUIRED");

            RuleFor(x => x.Prefix)
                .NotEmpty().WithErrorCode("USER.VALIDATION.PREFIX_REQUIRED")
                .Length(2, 5).WithErrorCode("USER.VALIDATION.PREFIX_LENGTH_INVALID");

            RuleFor(x => x.Role).IsInEnum().WithErrorCode("USER.VALIDATION.ROLE_INVALID");

            When(x => !string.IsNullOrEmpty(x.Password), () =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(6).WithErrorCode("USER.VALIDATION.PASSWORD_TOO_SHORT")
                    .MaximumLength(30).WithErrorCode("USER.VALIDATION.PASSWORD_TOO_LONG");
            });
        }
    }
}
