using Application.DTOs.Response;
using Domain.CustomExceptions;
using Domain.Models;
using Infrastructure.Context;
using MediatR;

namespace Application.Features.Cases.Commands
{
    public record UpdateWorkCaseCommand(int Amount, string Relation, Guid ForwarderId, Guid ClientId, Guid WorkCaseId) : IRequest<WorkCaseResponseDTO>;
    public class EditWorkCaseHandler(FormupContext context) : IRequestHandler<UpdateWorkCaseCommand, WorkCaseResponseDTO>
    {
        private readonly FormupContext _context = context;

        public async Task<WorkCaseResponseDTO> Handle(UpdateWorkCaseCommand request, CancellationToken ct)
        {
            var forwarder = await _context.Forwarders.FindAsync([request.ForwarderId, ct], ct) ?? throw new InstanceException(nameof(Forwarder), request.ForwarderId);
            var client = await _context.Clients.FindAsync([request.ClientId, ct], ct) ?? throw new InstanceException(nameof(Client), request.ClientId);
            var workCase = await _context.WorkCases.FindAsync([request.WorkCaseId, ct], ct) ?? throw new InstanceException(nameof(WorkCase), request.WorkCaseId);

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

            return new WorkCaseResponseDTO
            {
                Name = workCase.Name,
                Amount = workCase.Amount,
                Relation = workCase.Relation,
                IsAbandoned = workCase.IsAbandoned
            };
        }
    }
}
