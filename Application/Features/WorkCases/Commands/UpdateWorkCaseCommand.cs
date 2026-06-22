using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Commands
{
    public record UpdateWorkCaseCommand(int Amount, string Relation, Guid ForwarderId, Guid ClientId, Guid WorkCaseId) : IRequest<AppResult<WorkCaseResponse>>;
    public class EditWorkCaseHandler(FormupContext context) : IRequestHandler<UpdateWorkCaseCommand, AppResult<WorkCaseResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<WorkCaseResponse>> Handle(UpdateWorkCaseCommand request, CancellationToken ct)
        {
            var workCase = await _context.WorkCases.FindAsync([request.WorkCaseId], ct);
            if (workCase == null) return AppResult<WorkCaseResponse>.Failure("WORK_CASE.NOT_FOUND");

            var client = await _context.Clients.FindAsync([request.ClientId], ct);
            if (client == null) return AppResult<WorkCaseResponse>.Failure("CLIENT.NOT_FOUND");

            var forwarder = await _context.Forwarders.FindAsync([request.ForwarderId], ct);
            if (forwarder == null) return AppResult<WorkCaseResponse>.Failure("FORWARDER.NOT_FOUND");

            var totalAmountTaken = await _context.WorkCases
                .Where(x => x.Client.Id == client.Id && x.Id.Value != request.WorkCaseId && !x.IsAbandoned)
                .SumAsync(x => x.Amount, ct);

            if (!client.CanAssignAmount(request.Amount, totalAmountTaken, out var exceededBy))
            {
                return AppResult<WorkCaseResponse>.Failure(
                    "CLIENT.VALIDATION.CREDIT_EXCEEDED",
                    new { ExceededBy = exceededBy }
                );
            }

            if (workCase.Relation != request.Relation)
            {
                var nameParts = workCase.Name.Split('/');
                nameParts[0] = request.Relation;
                workCase.Name = string.Join("/", nameParts);
                workCase.Relation = request.Relation;
            }

            workCase.Amount = request.Amount;
            workCase.Forwarder = forwarder;
            workCase.Client = client;

            await _context.SaveChangesAsync(ct);

            var result = new WorkCaseResponse
            {
                Id = workCase.Id.Value,
                Name = workCase.Name,
                Amount = workCase.Amount,
                Relation = workCase.Relation,
                IsAbandoned = workCase.IsAbandoned
            };

            return AppResult<WorkCaseResponse>.Success(result);
        }
    }
}
