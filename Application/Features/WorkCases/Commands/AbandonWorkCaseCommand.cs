using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;

namespace Application.Features.WorkCases.Commands
{
    public record AbandonWorkCaseCommand(Guid WorkCaseId) : IRequest<AppResult<WorkCaseResponse>>;
    public class AbandonWorkCaseHandler(FormupContext context) : IRequestHandler<AbandonWorkCaseCommand, AppResult<WorkCaseResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<WorkCaseResponse>> Handle(AbandonWorkCaseCommand request, CancellationToken ct)
        {
            var workCase = await _context.WorkCases.FindAsync([request.WorkCaseId], ct);
            if (workCase == null) return AppResult<WorkCaseResponse>.Failure("WORK_CASE.NOT_FOUND");

            workCase.IsAbandoned = true;
            await _context.SaveChangesAsync(ct);

            var result = new WorkCaseResponse
            {
                Id = workCase.Id.Value,
                Name = workCase.Name,
                Amount = workCase.Amount,
                Relation = workCase.Relation,
                IsAbandoned = true,
            };

            return AppResult<WorkCaseResponse>.Success(result);
        }
    }
}
