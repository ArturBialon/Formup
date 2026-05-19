using Application.DTOs.Response;
using Azure.Core;
using Domain.CustomExceptions;
using Domain.Models;
using Infrastructure.Context;
using MediatR;

namespace Application.Features.Cases.Commands
{
    public record AbandonWorkCaseCommand(Guid WorkCaseId) : IRequest<WorkCaseResponseDTO>;
    public class AbandonWorkCaseHandler(FormupContext context) : IRequestHandler<AbandonWorkCaseCommand, WorkCaseResponseDTO>
    {
        private readonly FormupContext _context = context;

        public async Task<WorkCaseResponseDTO> Handle(AbandonWorkCaseCommand request, CancellationToken ct)
        {
            var workCase = await _context.WorkCases.FindAsync([request.WorkCaseId, ct], ct) ?? throw new InstanceException(nameof(WorkCase), request.WorkCaseId);
            if (workCase.Invoices.Count > 0) 
            {
                return new WorkCaseResponseDTO
                {
                    Name = workCase.Name,
                    Amount = workCase.Amount,
                    Relation = workCase.Relation,
                    IsAbandoned = false,
                    Message = "Work case cannot be abandoned because it has associated invoices."
                };
            }

            workCase.IsAbandoned = true;
            await _context.SaveChangesAsync(ct);

            return new WorkCaseResponseDTO
            {
                Name = workCase.Name,
                Amount = workCase.Amount,
                Relation = workCase.Relation,
                IsAbandoned = true,
                Message = "Work case has been successfully abandoned."
            };
        }
    }
}
