using Application.Common.FileStorage;
using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Costs.Commands
{
    public record UpdateCostCommand(
        Guid Id,
        IFormFile? File,
        decimal Amount,
        string Currency,
        decimal Tax,
        string Name,
        DateTime IssueDate,
        DateTime ServiceDate,
        Guid WorkCaseItemId,
        Guid ServiceContractorId
    ) : IRequest<IAppResult<Unit>>;

    public class UpdateCostCommandHandler(FormupContext context, IFileStorageService fileStorageService) : IRequestHandler<UpdateCostCommand, IAppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<IAppResult<Unit>> Handle(UpdateCostCommand request, CancellationToken ct)
        {
            var cost = await _context.Costs
                .Include(c => c.WorkCaseItem)
                .Include(c => c.ServiceContractor)
                .FirstOrDefaultAsync(x => x.Id.Value == request.Id, ct);

            if (cost == null) return AppResult<Unit>.Failure("COST.NOT_FOUND");

            var nameTaken = await _context.Costs.AnyAsync(x => x.Id.Value != request.Id && x.ServiceContractor.Id.Value == request.ServiceContractorId && x.Name == request.Name, ct);
            if (nameTaken) return AppResult<Unit>.Failure("COST.COST_ALREADY_EXISTS");

            if (cost.WorkCaseItem.Id.Value != request.WorkCaseItemId)
            {
                var newWorkCaseItem = await _context.WorkCaseItems.FirstOrDefaultAsync(x => x.Id.Value == request.WorkCaseItemId, ct);
                if (newWorkCaseItem == null) return AppResult<Unit>.Failure("COST.WORK_CASE_ITEM_NOT_FOUND");
                cost.WorkCaseItem = newWorkCaseItem;
            }

            if (cost.ServiceContractor.Id.Value != request.ServiceContractorId)
            {
                var newContractor = await _context.ServiceContractors.FirstOrDefaultAsync(x => x.Id.Value == request.ServiceContractorId, ct);
                if (newContractor == null) return AppResult<Unit>.Failure("COST.CONTRACTOR_NOT_FOUND");
                cost.ServiceContractor = newContractor;
            }

            string? oldUrlToDelete = null;

            if (request.File != null)
            {
                using var stream = request.File.OpenReadStream();
                cost.DocumentUrl = await _fileStorageService.UploadFileAsync(stream, request.Name, ct);
            }

            cost.Amount = request.Amount;
            cost.Currency = request.Currency.Trim().ToUpper();
            cost.Tax = request.Tax;
            cost.Name = request.Name.Trim();
            cost.IssueDate = request.IssueDate;
            cost.ServiceDate = request.ServiceDate;

            await _context.SaveChangesAsync(ct);

            if (!string.IsNullOrEmpty(oldUrlToDelete))
            {
                try
                {
                    await _fileStorageService.DeleteFileAsync(oldUrlToDelete, ct);
                }
                catch
                {
                    // Logujemy błąd (opcjonalnie), ale nie przerywamy operacji, 
                    // bo baza danych została już pomyślnie zaktualizowana.
                }
            }

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
