using Application.DTOs.Response;
using Domain.CustomExceptions;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Cases.Commands
{
    public record CreateWorkCaseCommand(int Amount, string Relation, Guid ForwarderId) : IRequest<WorkCaseResponseDTO>;
    public class CreateWorkCaseHandler(FormupContext context) : IRequestHandler<CreateWorkCaseCommand, WorkCaseResponseDTO>
    {
        private readonly FormupContext _context = context;

        public async Task<WorkCaseResponseDTO> Handle(CreateWorkCaseCommand request, CancellationToken ct)
        {
            var now = DateTime.Now;
            var forwarder = await _context.Forwarders.FindAsync([request.ForwarderId, ct], ct) ?? throw new InstanceException(nameof(Forwarder), request.ForwarderId);

            var monthlyWorkCaseAmount = await _context.WorkCases
                .CountAsync(x => x.Forwarder.Id == forwarder.Id
                && x.CreatedAt.Month == now.Month, ct);

            var name = $"{request.Relation}/{monthlyWorkCaseAmount + 1}/{forwarder.Prefix}/{DateTime.Now.Month}/{DateTime.Now.Year}";

            _context.WorkCases.Add(new WorkCase
            {
                Name = name,
                Amount = request.Amount,
                Relation = request.Relation,
                Forwarder = forwarder
            });

            await _context.SaveChangesAsync(ct);
            return new WorkCaseResponseDTO
            {
                Name = name,
                Amount = request.Amount,
                Relation = request.Relation
            };
        }
    }
}
