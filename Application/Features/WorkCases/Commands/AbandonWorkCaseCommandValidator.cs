using FluentValidation;
using Infrastructure.Context;

namespace Application.Features.WorkCases.Commands;

public class AbandonWorkCaseCommandValidator : AbstractValidator<AbandonWorkCaseCommand>
{
    private readonly FormupContext _context;

    public AbandonWorkCaseCommandValidator(FormupContext context)
    {
        _context = context;

        RuleFor(x => x.WorkCaseId)
            .NotEmpty().WithErrorCode("WORK_CASE.VALIDATION.ID.REQUIRED")
            .MustAsync(async (id, ct) =>
            {
                var exists = await _context.WorkCases.FindAsync([id], ct);
                return exists != null;
            }).WithErrorCode("WORK_CASE.VALIDATION.ID.NOT_FOUND");

        RuleFor(x => x)
            .MustAsync(async (command, ct) =>
            {
                var workCase = await _context.WorkCases.FindAsync([command.WorkCaseId], ct);
                if (workCase == null) return true;
                return workCase.Invoices.Count == 0 && workCase.Costs.Count == 0;
            }).WithErrorCode("WORK_CASE.VALIDATION.CANNOT_ABANDON");
    }
}

