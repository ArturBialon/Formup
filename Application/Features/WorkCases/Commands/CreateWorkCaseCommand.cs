using Application.Common.CurrencyServices;
using Application.Common.Results;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Commands
{
    public record CreateWorkCaseCommand(
        decimal Amount,
        string Relation,
        string CurrencyCode,
        Guid ForwarderId,
        Guid ClientId
        ) : IRequest<AppResult<Unit>>;

    public class CreateWorkCaseHandler(FormupContext context, ICurrencyConverterService currencyConverterService)
        : IRequestHandler<CreateWorkCaseCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrencyConverterService _currencyConverterService = currencyConverterService;

        public async Task<AppResult<Unit>> Handle(CreateWorkCaseCommand request, CancellationToken ct)
        {
            var forwarder = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(request.ForwarderId), ct);
            if (forwarder == null) return AppResult<Unit>.Failure("FORWARDER.NOT_FOUND");

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id.Equals(request.ClientId), ct);
            if (client == null) return AppResult<Unit>.Failure("CLIENT.NOT_FOUND");
            if (!client.IsActive) return AppResult<Unit>.Failure("CLIENT.IS_INACTIVE");

            var requestedAmountInPln = await _currencyConverterService.ConvertToTargetCurrency(request.Amount, request.CurrencyCode, "PLN", DateTime.UtcNow, ct);
            var totalAmountTakenInPln = await _context.WorkCases
                            .Where(x => x.Client.Id.Equals(client.Id) && !x.IsAbandoned)
                            .Select(wc => new
                            {
                                wc.AmountInPln,
                                PaidAmount = wc.Invoices.Where(i => i.IsPaid).Sum(i => (decimal?)i.AmountInPln) ?? 0m
                            })
                            .SumAsync(x => x.AmountInPln - x.PaidAmount, ct);

            if (!Client.CanAssignAmount(requestedAmountInPln.Value, totalAmountTakenInPln, client.CreditInPln, out var exceededBy))
            {
                var amountInTargetCurrency = await _currencyConverterService.ConvertToTargetCurrency(exceededBy, "PLN", request.CurrencyCode, DateTime.UtcNow, ct);

                return AppResult<Unit>.Failure(
                    "CLIENT.VALIDATION.CREDIT_EXCEEDED",
                    new { ExceededBy = amountInTargetCurrency.Value, request.CurrencyCode }
                );
            }

            var name = await CreateWorkCaseNameAsync(request, forwarder, ct);

            var workCase = new WorkCase
            {
                Name = name,
                Amount = request.Amount,
                AmountInPln = requestedAmountInPln.Value,
                CurrencyCode = request.CurrencyCode,
                Relation = request.Relation,
                Forwarder = forwarder,
                Client = client
            };

            var created = _context.WorkCases.Add(workCase);
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }

        private async Task<string> CreateWorkCaseNameAsync(CreateWorkCaseCommand request, User forwarder, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var monthlyWorkCaseAmount = await _context.WorkCases
                .CountAsync(x => x.Forwarder.Id == forwarder.Id && x.CreatedAtUtc.Month == now.Month, ct);

            return $"{request.Relation}/{monthlyWorkCaseAmount + 1}/{forwarder.Prefix}/{now.Month}/{now.Year}";
        }
    }
}
