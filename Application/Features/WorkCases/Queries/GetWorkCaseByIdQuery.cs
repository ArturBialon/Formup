using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Queries
{
    public record GetWorkCaseByIdQuery(Guid Id) : IRequest<AppResult<WorkCaseResponse>>;

    public class GetWorkCaseByIdHandler(FormupContext context) : IRequestHandler<GetWorkCaseByIdQuery, AppResult<WorkCaseResponse>>
    {
        public async Task<AppResult<WorkCaseResponse>> Handle(GetWorkCaseByIdQuery request, CancellationToken ct)
        {
            var result = await context.WorkCases
                .AsNoTracking()
                .Where(x => x.Id.Value == request.Id)
                .Select(x => new WorkCaseResponse
                {
                    Id = x.Id.Value,
                    Name = x.Name,
                    Amount = x.Amount,
                    Relation = x.Relation,
                    ForwarderId = x.Forwarder.Id,
                    ForwarderName = $"{x.Forwarder.Name} {x.Forwarder.Surname}",
                    ClientId = x.Client.Id,
                    ClientName = x.Client.Name
                })
                .FirstOrDefaultAsync(ct);

            if (result == null) return AppResult<WorkCaseResponse>.Failure("WORK_CASE.NOT_FOUND");

            return AppResult<WorkCaseResponse>.Success(result);
        }
    }
}