using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Costs.Queries
{
    public record GetCostByIdQuery(Guid CostId) : IRequest<AppResult<CostDetailResponse>>;

    public class GetCostByIdQueryHandler(FormupContext context) : IRequestHandler<GetCostByIdQuery, AppResult<CostDetailResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<CostDetailResponse>> Handle(GetCostByIdQuery request, CancellationToken ct)
        {
            var costDto = await _context.Costs
                .AsNoTracking()
                .Where(x => x.Id.Equals(request.CostId))
                .Select(c => new CostDetailResponse
                {
                    Id = c.Id.Value,
                    Amount = c.Amount,
                    Currency = c.CurrencyCode,
                    Tax = c.Tax,
                    Name = c.Name,
                    IssueDate = c.IssueDate,
                    ServiceDate = c.ServiceDate,
                    DocumentUrl = c.DocumentUrl,
                    IsPaid = c.IsPaid,
                    ServiceContractorName = c.ServiceContractor.Name + ": " + c.ServiceContractor.Tax,
                    WorkCaseItemId = c.WorkCaseItem.Id.Value,
                    ServiceContractorId = c.ServiceContractor.Id.Value
                })
                .FirstOrDefaultAsync(ct);

            if (costDto == null) return AppResult<CostDetailResponse>.Failure("COST.NOT_FOUND");

            return AppResult<CostDetailResponse>.Success(costDto);
        }
    }
}
