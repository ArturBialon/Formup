using FluentValidation;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ServiceContractors.Commands
{
    public class CreateServiceContractorCommandValidator : AbstractValidator<CreateServiceContractorCommand>
    {
        private readonly FormupContext _context;

        public CreateServiceContractorCommandValidator(FormupContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode("CONTRACTOR.VALIDATION.NAME.REQUIRED")
                .MaximumLength(150).WithErrorCode("CONTRACTOR.VALIDATION.NAME.TOO_LONG");

            RuleFor(x => x.Tax)
                .NotEmpty().WithErrorCode("CONTRACTOR.VALIDATION.TAX.REQUIRED")
                .MaximumLength(20).WithErrorCode("CONTRACTOR.VALIDATION.TAX.TOO_LONG")
                .MustAsync(BeUniqueTax).WithErrorCode("CONTRACTOR.VALIDATION.TAX.NOT_UNIQUE");

            RuleFor(x => x.Country)
                .NotEmpty().WithErrorCode("CONTRACTOR.VALIDATION.COUNTRY.REQUIRED")
                .MaximumLength(54).WithErrorCode("CONTRACTOR.VALIDATION.COUNTRY.TOO_LONG");

            RuleFor(x => x.City)
                .NotEmpty().WithErrorCode("CONTRACTOR.VALIDATION.CITY.REQUIRED")
                .MaximumLength(100).WithErrorCode("CONTRACTOR.VALIDATION.CITY.TOO_LONG");

            RuleFor(x => x.Zip)
                .NotEmpty().WithErrorCode("CONTRACTOR.VALIDATION.ZIP.REQUIRED")
                .MaximumLength(10).WithErrorCode("CONTRACTOR.VALIDATION.ZIP.TOO_LONG");

            RuleFor(x => x.Street)
                .NotEmpty().WithErrorCode("CONTRACTOR.VALIDATION.STREET.REQUIRED")
                .MaximumLength(100).WithErrorCode("CONTRACTOR.VALIDATION.STREET.TOO_LONG");

            RuleFor(x => x.HouseNumber)
                .NotEmpty().WithErrorCode("CONTRACTOR.VALIDATION.HOUSE_NUMBER.REQUIRED")
                .MaximumLength(10).WithErrorCode("CONTRACTOR.VALIDATION.HOUSE_NUMBER.TOO_LONG");

            RuleFor(x => x.ApartmentNumber)
                .MaximumLength(10).WithErrorCode("CONTRACTOR.VALIDATION.APARTMENT_NUMBER.TOO_LONG")
                .When(x => x.ApartmentNumber != null);

            RuleFor(x => x.Email)
                .EmailAddress().WithErrorCode("CONTRACTOR.VALIDATION.EMAIL.INVALID_FORMAT")
                .MaximumLength(254).WithErrorCode("CONTRACTOR.VALIDATION.EMAIL.TOO_LONG")
                .When(x => x.Email != null);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithErrorCode("CONTRACTOR.VALIDATION.PHONE_NUMBER.TOO_LONG")
                .When(x => x.PhoneNumber != null);
        }

        private async Task<bool> BeUniqueTax(string tax, CancellationToken ct)
        {
            var exists = await _context.ServiceContractors.AnyAsync(x => x.Tax == tax, ct);
            return !exists;
        }
    }
}