using Application.Common.Results;
using Application.DTOs.Response;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Commands
{
    public record CreateWorkCaseCommand(int Amount, string Relation, Guid ForwarderId, Guid ClientId)
        : IRequest<AppResult<WorkCaseResponse>>;

    public class CreateWorkCaseHandler(FormupContext context)
        : IRequestHandler<CreateWorkCaseCommand, AppResult<WorkCaseResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<WorkCaseResponse>> Handle(CreateWorkCaseCommand request, CancellationToken ct)
        {
            var forwarder = await _context.Forwarders.FindAsync([request.ForwarderId], ct);
            if (forwarder == null) return AppResult<WorkCaseResponse>.Failure("FORWARDER.NOT_FOUND");

            var client = await _context.Clients.FindAsync([request.ClientId], ct);
            if (client == null) return AppResult<WorkCaseResponse>.Failure("CLIENT.NOT_FOUND");

            var totalAmountTaken = await _context.WorkCases
                .Where(x => x.Client.Id == client.Id && !x.IsAbandoned)
                .SumAsync(x => x.Amount, ct);

            var availableCredit = client.Credit - totalAmountTaken;

            if (request.Amount > availableCredit)
            {
                return AppResult<WorkCaseResponse>.Failure(
                    "CLIENT.VALIDATION.CREDIT_EXCEEDED",
                    new { ExceededBy = request.Amount - availableCredit }
                );
            }

            var name = await CreateWorkCaseNameAsync(request, forwarder, ct);

            var workCase = new WorkCase
            {
                Name = name,
                Amount = request.Amount,
                Relation = request.Relation,
                Forwarder = forwarder,
                Client = client
            };

            var created = _context.WorkCases.Add(workCase);
            await _context.SaveChangesAsync(ct);

            var result = new WorkCaseResponse
            {
                Id = created.Entity.Id.Value,
                Name = name,
                Amount = request.Amount,
                Relation = request.Relation,
                ForwarderId = forwarder.Id.Value,
                ForwarderName = forwarder.Surname,
                ClientId = client.Id.Value,
                ClientName = client.Name,
                IsAbandoned = false
            };

            return AppResult<WorkCaseResponse>.Success(result);
        }

        private async Task<string> CreateWorkCaseNameAsync(CreateWorkCaseCommand request, Forwarder forwarder, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var monthlyWorkCaseAmount = await _context.WorkCases
                .CountAsync(x => x.Forwarder.Id == forwarder.Id && x.CreatedAt.Month == now.Month, ct);

            return $"{request.Relation}/{monthlyWorkCaseAmount + 1}/{forwarder.Prefix}/{now.Month}/{now.Year}";
        }
    }
}
