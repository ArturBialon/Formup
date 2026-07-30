using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Commands
{
    public record AbandonWorkCaseCommand(Guid WorkCaseId) : IRequest<AppResult<Unit>>;

    public class AbandonWorkCaseHandler(FormupContext context) : IRequestHandler<AbandonWorkCaseCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<Unit>> Handle(AbandonWorkCaseCommand request, CancellationToken ct)
        {
            var workCase = await _context.WorkCases
                .Include(x => x.Invoices)
                .Include(x => x.WorkCaseItems)
                .FirstOrDefaultAsync(wc => wc.Id.Equals(request.WorkCaseId), cancellationToken: ct);
            if (workCase == null) return AppResult<Unit>.Failure("WORK_CASE.NOT_FOUND");
            if (workCase.Invoices.Count != 0) return AppResult<Unit>.Failure("WORK_CASE.CANNOT_ABANDON_INVOICED");
            if (workCase.WorkCaseItems.Count != 0) return AppResult<Unit>.Failure("WORK_CASE.CANNOT_ABANDON_HAS_ITEMS");

            workCase.IsAbandoned = true;
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
