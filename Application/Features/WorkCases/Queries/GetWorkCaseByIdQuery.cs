using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Queries
{
    public record GetWorkCaseByIdQuery(Guid Id) : IRequest<WorkCaseResponse?>;

    public class GetWorkCaseByIdHandler(FormupContext context) : IRequestHandler<GetWorkCaseByIdQuery, WorkCaseResponse?>
    {
        public async Task<WorkCaseResponse?> Handle(GetWorkCaseByIdQuery request, CancellationToken ct)
        {
            return await context.WorkCases
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
        }
    }
}