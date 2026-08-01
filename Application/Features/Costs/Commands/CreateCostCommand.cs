using Application.Common.FileStorage;
using Application.Common.Results;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Costs.Commands
{
    public record CreateCostCommand(
        decimal Amount,
        string Currency,
        decimal Tax,
        string Name,
        DateTime IssueDate,
        DateTime ServiceDate,
        bool IsPaid,
        Guid WorkCaseItemId,
        Guid ServiceContractorId
    ) : IRequest<AppResult<Guid>>;

    public class CreateCostCommandHandler(FormupContext context, IFileStorageService fileStorageService) : IRequestHandler<CreateCostCommand, AppResult<Guid>>
    {
        private readonly FormupContext _context = context;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<AppResult<Guid>> Handle(CreateCostCommand request, CancellationToken ct)
        {
            var workCaseItem = await _context.WorkCaseItems
                .FirstOrDefaultAsync(x => x.Id.Equals(request.WorkCaseItemId), ct);
            var contractor = await _context.ServiceContractors
                .FirstOrDefaultAsync(x => x.Id.Equals(request.ServiceContractorId), ct);
            var existingCost = await _context.Costs
                .FirstOrDefaultAsync(x => x.Name == request.Name && x.ServiceContractor.Id.Equals(request.ServiceContractorId), ct);
            var existingCostInItem = await _context.Costs
                .FirstOrDefaultAsync(c => c.WorkCaseItem.Id.Equals(request.WorkCaseItemId), ct);

            if (workCaseItem == null)
                return AppResult<Guid>.Failure("COST.WORK_CASE_ITEM_NOT_FOUND");
            if (contractor == null)
                return AppResult<Guid>.Failure("COST.CONTRACTOR_NOT_FOUND");
            if (!contractor.IsActive)
                return AppResult<Guid>.Failure("COST.CONTRACTOR_IS_INACTIVE");
            if (existingCost != null)
                return AppResult<Guid>.Failure("COST.COST_ALREADY_EXISTS");
            if (existingCostInItem != null)
                return AppResult<Guid>.Failure("COST.WORK_CASE_ITEM_HAS_COST");
            if (workCaseItem.CostAmountNet != request.Amount || workCaseItem.CurrencyCodeCost != request.Currency)
                return AppResult<Guid>.Failure("COST.WORK_CASE_ITEM_AMOUNT_MISSMATCH");

            string uploadedUrl = string.Empty;

            var cost = new Cost
            {
                Amount = request.Amount,
                CurrencyCode = request.Currency.Trim().ToUpper(),
                Tax = request.Tax,
                Name = request.Name.Trim(),
                IssueDate = request.IssueDate,
                ServiceDate = request.ServiceDate,
                WorkCaseItem = workCaseItem,
                ServiceContractor = contractor,
                DocumentUrl = uploadedUrl,
                IsPaid = request.IsPaid
            };

            _context.Costs.Add(cost);
            await _context.SaveChangesAsync(ct);

            return AppResult<Guid>.Success(cost.Id.Value);
        }
    }
}
