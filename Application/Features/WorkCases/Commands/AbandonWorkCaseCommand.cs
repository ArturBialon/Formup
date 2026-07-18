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
            var workCase = await _context.WorkCases.FirstOrDefaultAsync(wc => wc.Id.Equals(request.WorkCaseId), cancellationToken: ct);
            if (workCase == null) return AppResult<Unit>.Failure("WORK_CASE.NOT_FOUND");

            workCase.IsAbandoned = true;
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
