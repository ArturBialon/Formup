using Application.DTOs.Response;
using Azure.Core;
using Domain.CustomExceptions;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Cases.Commands
{
    public record CreateWorkCaseCommand(int Amount, string Relation, Guid ForwarderId, Guid ClientId) : IRequest<WorkCaseResponseDTO>;
    public class CreateWorkCaseHandler(FormupContext context) : IRequestHandler<CreateWorkCaseCommand, WorkCaseResponseDTO>
    {
        private readonly FormupContext _context = context;

        public async Task<WorkCaseResponseDTO> Handle(CreateWorkCaseCommand request, CancellationToken ct)
        {
            var forwarder = await _context.Forwarders.FindAsync([request.ForwarderId, ct], ct) ?? throw new InstanceException(nameof(Forwarder), request.ForwarderId);
            var client = await _context.Clients.FindAsync([request.ClientId, ct], ct) ?? throw new InstanceException(nameof(Client), request.ClientId);
            
            var name = await CreateWorkCaseName(request, forwarder, ct);

            _context.WorkCases.Add(new WorkCase
            {
                Name = name,
                Amount = request.Amount,
                Relation = request.Relation,
                Forwarder = forwarder,
                Client = client
            });

            await _context.SaveChangesAsync(ct);
            return new WorkCaseResponseDTO
            {
                Name = name,
                Amount = request.Amount,
                Relation = request.Relation,
                IsAbandoned = false
            };
        }

        public async Task<string> CreateWorkCaseName(CreateWorkCaseCommand request, Forwarder forwarder, CancellationToken ct)
        {
            var now = DateTime.Now;
            var monthlyWorkCaseAmount = await _context.WorkCases
                .CountAsync(x => x.Forwarder.Id == forwarder.Id
                && x.CreatedAt.Month == now.Month, ct);

            return $"{request.Relation}/{monthlyWorkCaseAmount + 1}/{forwarder.Prefix}/{DateTime.Now.Month}/{DateTime.Now.Year}";
        }
    }
}
