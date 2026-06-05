using Application.Common;
using FluentValidation;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Commands;

public class UpdateWorkCaseCommandValidator : AbstractValidator<UpdateWorkCaseCommand>
{
    private readonly FormupContext _context;

    public UpdateWorkCaseCommandValidator(FormupContext context)
    {
        _context = context;

        RuleFor(x => x.Relation).MustBeValidRelation();

        RuleFor(x => x)
            .CustomAsync(async (cmd, context, ct) =>
            {
                var client = await _context.Clients.FindAsync([cmd.ClientId], ct);
                if (client == null) return;

                var totalAmountTaken = await _context.WorkCases
                    .Where(x => x.Client.Id == client.Id && x.Id.Value != cmd.WorkCaseId && !x.IsAbandoned)
                    .SumAsync(x => x.Amount, ct);

                var availableCredit = client.Credit - totalAmountTaken;

                if (cmd.Amount > availableCredit)
                {
                    var exceededBy = cmd.Amount - availableCredit;

                    context.AddFailure(
                        nameof(cmd.Amount),
                        $"Client's credit limit exceeded by: {exceededBy} PLN."
                    );
                }
            });
    }
}