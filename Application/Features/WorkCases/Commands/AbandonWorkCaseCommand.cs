using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;

namespace Application.Features.WorkCases.Commands
{
    public record AbandonWorkCaseCommand(Guid WorkCaseId) : IRequest<WorkCaseResponse>;
    public class AbandonWorkCaseHandler(FormupContext context) : IRequestHandler<AbandonWorkCaseCommand, WorkCaseResponse?>
    {
        private readonly FormupContext _context = context;

        public async Task<WorkCaseResponse?> Handle(AbandonWorkCaseCommand request, CancellationToken ct)
        {
            var workCase = await _context.WorkCases.FindAsync([request.WorkCaseId], ct);
            if (workCase == null) return null;

            workCase.IsAbandoned = true;
            await _context.SaveChangesAsync(ct);

            return new WorkCaseResponse
            {
                Id = workCase.Id.Value,
                Name = workCase.Name,
                Amount = workCase.Amount,
                Relation = workCase.Relation,
                IsAbandoned = true,
                Message = "Work case has been successfully abandoned."
            };
        }
    }
}
