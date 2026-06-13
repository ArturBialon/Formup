using Domain.Constants;
using FluentValidation;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Commands;

public class CreateServiceContractorCommandValidator : AbstractValidator<CreateWorkCaseCommand>
{
    private readonly FormupContext _context;

    public CreateServiceContractorCommandValidator(FormupContext context)
    {
        _context = context;

        RuleFor(x => x.Relation)
            .NotEmpty()
            .WithErrorCode("WORK_CASE.VALIDATION.RELATION.REQUIRED")
            .Must(relation => Relations.All.Contains(relation))
            .WithErrorCode("WORK_CASE.VALIDATION.RELATION.INVALID")
            .WithState(_ => new { AllowedRelations = Relations.All });

        RuleFor(x => x.Amount)
            .NotEmpty().WithErrorCode("WORK_CASE.VALIDATION.AMOUNT.REQUIRED")
            .GreaterThan(0).WithErrorCode("WORK_CASE.VALIDATION.AMOUNT.MUST_BE_POSITIVE");

        RuleFor(x => x.ForwarderId)
            .NotEmpty().WithErrorCode("WORK_CASE.VALIDATION.FORWARDER_NOT_FOUND");

        RuleFor(x => x.ClientId)
            .NotEmpty().WithErrorCode("WORK_CASE.VALIDATION.CLIENT_NOT_FOUND");

        RuleFor(x => x)
            .CustomAsync(async (cmd, context, ct) =>
            {
                var client = await _context.Clients.FindAsync([cmd.ClientId], ct);
                if (client == null) return;

                var totalAmountTaken = await _context.WorkCases
                    .Where(x => x.Client.Id == client.Id && !x.IsAbandoned)
                    .SumAsync(x => x.Amount, ct);

                var availableCredit = client.Credit - totalAmountTaken;

                if (cmd.Amount > availableCredit)
                {
                    var exceededBy = cmd.Amount - availableCredit;

                    var failure = new FluentValidation.Results.ValidationFailure(
                        nameof(cmd.Amount),
                        "CLIENT.VALIDATION.CREDIT_EXCEEDED"
                    )
                    {
                        ErrorCode = "CLIENT.VALIDATION.CREDIT_EXCEEDED",
                        CustomState = new { ExceededBy = exceededBy }
                    };

                    context.AddFailure(failure);
                }
            });

    }
}