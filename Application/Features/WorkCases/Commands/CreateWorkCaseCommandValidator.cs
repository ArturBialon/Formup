using Domain.Constants;
using FluentValidation;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Commands
{
    public class CreateWorkCaseCommandValidator : AbstractValidator<CreateWorkCaseCommand>
    {
        public CreateWorkCaseCommandValidator()
        {
            RuleFor(x => x.Relation)
                .NotEmpty().WithErrorCode("WORK_CASE.VALIDATION.RELATION.REQUIRED")
                .Must(relation => Relations.All.Contains(relation)).WithErrorCode("WORK_CASE.VALIDATION.RELATION.INVALID")
                .WithState(_ => new { AllowedRelations = Relations.All });

            RuleFor(x => x.Amount)
                .NotEmpty().WithErrorCode("WORK_CASE.VALIDATION.AMOUNT.REQUIRED")
                .GreaterThan(0).WithErrorCode("WORK_CASE.VALIDATION.AMOUNT.MUST_BE_POSITIVE");

            RuleFor(x => x.ForwarderId)
                .NotEmpty().WithErrorCode("WORK_CASE.VALIDATION.FORWARDER_ID_REQUIRED");

            RuleFor(x => x.ClientId)
                .NotEmpty().WithErrorCode("WORK_CASE.VALIDATION.CLIENT_ID_REQUIRED");
        }
    }
}