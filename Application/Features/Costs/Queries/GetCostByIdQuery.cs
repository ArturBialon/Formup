using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Costs.Queries
{
    public record GetCostByIdQuery(Guid Id) : IRequest<IAppResult<CostDetailResponse>>;

    public class GetCostByIdQueryHandler(FormupContext context) : IRequestHandler<GetCostByIdQuery, IAppResult<CostDetailResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<IAppResult<CostDetailResponse>> Handle(GetCostByIdQuery request, CancellationToken ct)
        {
            var costDto = await _context.Costs
                .AsNoTracking()
                .Where(x => x.Id.Value == request.Id)
                .Select(c => new CostDetailResponse
                {
                    Id = c.Id.Value,
                    Amount = c.Amount,
                    Currency = c.Currency,
                    Tax = c.Tax,
                    Name = c.Name,
                    IssueDate = c.IssueDate,
                    ServiceDate = c.ServiceDate,
                    DocumentUrl = c.DocumentUrl,
                    WorkCaseItemId = c.WorkCaseItem.Id.Value,
                    ServiceContractorId = c.ServiceContractor.Id.Value
                })
                .FirstOrDefaultAsync(ct);

            if (costDto == null)
            {
                return AppResult<CostDetailResponse>.Failure("COST.NOT_FOUND");
            }

            return AppResult<CostDetailResponse>.Success(costDto);
        }
    }
}
