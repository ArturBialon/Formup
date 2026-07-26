using Application.Common.FileStorage;
using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Costs.Commands
{
    public record UploadCostFileCommand(
        Guid CostId,
        IFormFile File
    ) : IRequest<AppResult<string>>;

    public class UploadCostFileCommandHandler(
        FormupContext context,
        IFileStorageService fileStorageService
    ) : IRequestHandler<UploadCostFileCommand, AppResult<string>>
    {
        private readonly FormupContext _context = context;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<AppResult<string>> Handle(UploadCostFileCommand request, CancellationToken ct)
        {
            if (request.File == null || request.File.Length == 0)
                return AppResult<string>.Failure("COST.FILE_IS_EMPTY");

            var cost = await _context.Costs
                .FirstOrDefaultAsync(x => x.Id.Equals(request.CostId), ct);

            if (cost == null)
                return AppResult<string>.Failure("COST.NOT_FOUND");

            //var uploadedUrl = await _fileStorageService.UploadFileAsync(request.File, cost.Name + "_" + cost.ServiceContractor.Tax, ct);
            //cost.DocumentUrl = uploadedUrl;

            _context.Costs.Update(cost);
            await _context.SaveChangesAsync(ct);

            return AppResult<string>.Success(cost.DocumentUrl ?? "Doc_url");
        }
    }
}