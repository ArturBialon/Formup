using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Queries
{
    public record GetWorkCaseByIdQuery(Guid Id) : IRequest<AppResult<WorkCaseDetailsResponse>>;

    public class GetWorkCaseByIdHandler(FormupContext context) : IRequestHandler<GetWorkCaseByIdQuery, AppResult<WorkCaseDetailsResponse>>
    {
        public async Task<AppResult<WorkCaseDetailsResponse>> Handle(GetWorkCaseByIdQuery request, CancellationToken ct)
        {
            var result = await context.WorkCases
                .AsNoTracking()
                .Where(x => x.Id.Equals(request.Id))
                .Select(x => new WorkCaseDetailsResponse
                {
                    Id = x.Id.Value,
                    Name = x.Name,
                    Amount = x.Amount,
                    AmountInPln = x.AmountInPln,
                    CreatedAtUtc = x.CreatedAtUtc,
                    Currency = x.CurrencyCode,
                    Relation = x.Relation,
                    ForwarderId = x.Forwarder.Id,
                    ForwarderName = $"{x.Forwarder.Name} {x.Forwarder.Surname}",
                    ClientId = x.Client.Id,
                    ClientName = x.Client.Name,
                    IsAbandoned = x.IsAbandoned,
                })
                .FirstOrDefaultAsync(ct);

            if (result == null) return AppResult<WorkCaseDetailsResponse>.Failure("WORK_CASE.NOT_FOUND");

            return AppResult<WorkCaseDetailsResponse>.Success(result);
        }
    }
}