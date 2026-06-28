using Application.Common.FileStorage;
using Application.Common.Results;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Costs.Commands
{
    public record CreateCostCommand(
        IFormFile? File,
        decimal Amount,
        string Currency,
        decimal Tax,
        string Name,
        DateTime IssueDate,
        DateTime ServiceDate,
        Guid WorkCaseItemId,
        Guid ServiceContractorId
    ) : IRequest<IAppResult<Guid>>;

    public class CreateCostCommandHandler(FormupContext context, IFileStorageService fileStorageService) : IRequestHandler<CreateCostCommand, IAppResult<Guid>>
    {
        private readonly FormupContext _context = context;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<IAppResult<Guid>> Handle(CreateCostCommand request, CancellationToken ct)
        {
            var workCaseItem = await _context.WorkCaseItems
                .FirstOrDefaultAsync(x => x.Id.Value == request.WorkCaseItemId, ct);
            var contractor = await _context.ServiceContractors
                .FirstOrDefaultAsync(x => x.Id.Value == request.ServiceContractorId, ct);
            var existingCost = await _context.Costs
                .FirstOrDefaultAsync(x => x.Name == request.Name && x.ServiceContractor.Id.Value == request.ServiceContractorId, ct);

            if (workCaseItem == null)
                return AppResult<Guid>.Failure("COST.VALIDATION.WORK_CASE_ITEM_NOT_FOUND");
            if (contractor == null)
                return AppResult<Guid>.Failure("COST.VALIDATION.CONTRACTOR_NOT_FOUND");
            if (existingCost != null)
                return AppResult<Guid>.Failure("COST.VALIDATION.COST_ALREADY_EXISTS");

            string uploadedUrl = string.Empty;

            if (request.File != null)
            {
                using var stream = request.File.OpenReadStream();
                uploadedUrl = await _fileStorageService.UploadFileAsync(stream, request.Name, ct);
            }

            var cost = new Cost
            {
                Amount = request.Amount,
                Currency = request.Currency.Trim().ToUpper(),
                Tax = request.Tax,
                Name = request.Name.Trim(),
                IssueDate = request.IssueDate,
                ServiceDate = request.ServiceDate,
                WorkCaseItem = workCaseItem,
                ServiceContractor = contractor,
                DocumentUrl = uploadedUrl
            };

            _context.Costs.Add(cost);
            await _context.SaveChangesAsync(ct);

            return AppResult<Guid>.Success(cost.Id.Value);
        }
    }
}
