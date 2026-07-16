using Application.Common.CurrencyServices;
using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Commands
{
    public record UpdateWorkCaseCommand(
        decimal Amount,
        string CurrencyCode,
        string Relation,
        Guid ForwarderId,
        Guid ClientId,
        Guid WorkCaseId
        ) : IRequest<AppResult<Unit>>;

    public class EditWorkCaseHandler(FormupContext context, ICurrencyConverterService currencyConverterService) : IRequestHandler<UpdateWorkCaseCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrencyConverterService _currencyConverterService = currencyConverterService;

        public async Task<AppResult<Unit>> Handle(UpdateWorkCaseCommand request, CancellationToken ct)
        {
            var workCase = await _context.WorkCases.FirstOrDefaultAsync(wc => wc.Id.Equals(request.WorkCaseId), ct);
            if (workCase == null) return AppResult<Unit>.Failure("WORK_CASE.NOT_FOUND");

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id.Equals(request.ClientId), ct);
            if (client == null) return AppResult<Unit>.Failure("CLIENT.NOT_FOUND");

            var forwarder = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(request.ForwarderId), ct);
            if (forwarder == null) return AppResult<Unit>.Failure("FORWARDER.NOT_FOUND");

            if (request.Amount != workCase.Amount || request.CurrencyCode != workCase.CurrencyCode)
            {
                var requestedAmountInPln = await _currencyConverterService.ConvertToTargetCurrency(request.Amount, request.CurrencyCode, "PLN", DateTime.UtcNow, ct);

                var totalAmountTaken = await _context.WorkCases
                                .Where(x => x.Client.Id == client.Id && !x.Id.Equals(request.WorkCaseId) && !x.IsAbandoned)
                                .Select(wc => new
                                {
                                    wc.AmountInPln,
                                    PaidAmount = wc.Invoices.Where(i => i.IsPaid).Sum(i => (decimal?)i.AmountInPln) ?? 0m
                                })
                                .SumAsync(x => x.AmountInPln - x.PaidAmount, ct);

                if (!client.CanAssignAmount(requestedAmountInPln.Value, totalAmountTaken, out var exceededBy))
                {
                    return AppResult<Unit>.Failure(
                        "CLIENT.VALIDATION.CREDIT_EXCEEDED",
                        new { ExceededBy = exceededBy }
                    );
                }

                workCase.Amount = request.Amount;
                workCase.CurrencyCode = request.CurrencyCode;
                workCase.AmountInPln = requestedAmountInPln.Value;
            }

            if (workCase.Relation != request.Relation)
            {
                var nameParts = workCase.Name.Split('/');
                nameParts[0] = request.Relation;
                workCase.Name = string.Join("/", nameParts);
                workCase.Relation = request.Relation;
            }

            workCase.Forwarder = forwarder;
            workCase.Client = client;

            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
